using Bird_Box.Audio;
using Bird_Box.Data;
using Bird_Box.Models;
using Bird_Box.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Bird_Box.Controllers
{
    [ApiController]
    public class ResultsAPIController : ControllerBase
    {
        private readonly BirdRepository _dbOperations;
        private readonly MicrophoneRepository _microphoneDbOperations;

        //private readonly AnalyzerOptions _defaultOptions;
        //private readonly IConfigurationRoot _config;

        public ResultsAPIController(BirdRepository dbOperations, MicrophoneRepository mics)
        {
            _dbOperations = dbOperations;
            _microphoneDbOperations = mics;
        }

        /// <summary>
        /// Get detection item by its ID
        /// </summary>
        /// <param name="recordId">Detection ID</param>
        /// <returns></returns>
        /// <response code="200">Returns the detection</response>
        /// <response code="404">The detection was not found</response>
        [HttpGet("api/results/{recordId}")]
        public async Task<IActionResult> GetByID([FromRoute] string recordId)
        {
            var bird = await _dbOperations.GetById(recordId);
            if (bird == null)
                return NotFound();
            return Ok(bird);
        }

        /// <summary>
        /// Get all detections in a local database
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return all database content</response>
        [HttpGet("api/results")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate);
            return Ok(results);
        }

        /// <summary>
        /// Get all detections by a certain day
        /// </summary>
        /// <param name="yyyyMMdd">Date in format yyyy-MM-dd, i.e. 2023-10-22</param>
        /// <returns></returns>
        /// <response code="200">Return detections in a certain day</response>
        [HttpGet("api/results/days/{yyyyMMdd}")]
        public async Task<IActionResult> GetAllDetectionsByDay([FromRoute] DateTime yyyyMMdd)
        {
            DateTime date = yyyyMMdd;
            //DateTime.TryParse(yyyyMMdd, out date);
            var results = await _dbOperations.GetByDate(date);
            results.OrderByDescending(x => x.recodingDate.Ticks);
            return Ok(results);
        }

        /// <summary>
        /// Get all detections of a certain species
        /// </summary>
        /// <param name="birdName">An exact common name of a bird, i.e. "Eurasian blue tit" instead of "Cyanistes caeruleus"</param>
        /// <returns></returns>
        /// <response code="200">Returns all birds with a name provided</response>
        /// <response code="404">No birds with a name provided were found</response>
        [HttpGet("api/results/birds/{birdName}")]
        public async Task<IActionResult> GetByName([FromRoute] string birdName)
        {
            var records = await _dbOperations.GetByName(birdName);
            if (records == null)
                return NotFound();
            return Ok(records);
        }

        /// <summary>
        /// Process all raw results and put them to a database.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Process was finished successfully</response>
        [HttpGet("api/results/process")]
        public async Task<IActionResult> ProcessResults()
        {
            var devices = await _microphoneDbOperations.GetAll(); //CommandLine.GetAudioDevices();
            int totalResults = 0;
            foreach (var device in devices)
            {
                totalResults += await ProcessResultsByDevice(device);
            }
            return Ok($"Added {totalResults} detections.");
        }

        /// <summary>
        /// Process all results recorded from a certain input device (with provided input device ID)
        /// </summary>
        /// <param name="inputDeviceID">Input device ID</param>
        /// <returns>Number of detections</returns>
        //[HttpGet("api/results/process/{inputDeviceID}")]
        private async Task<int> ProcessResultsByDevice(Microphone inputDevice)
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing(
                $"Microphone-{inputDevice.deviceId}/"
            );
            var results = rrp.ProcessAllFiles();
            List<IdentifiedBird> detections = new List<IdentifiedBird>();
            foreach (var result in results)
            {
                var detection = result;
                detection.inputDevice = inputDevice;
                detections.Add(detection);
                Console.WriteLine($"Input device: {inputDevice.deviceInfo} {inputDevice.deviceId}");
            }
            var count = await _dbOperations.CreateRange(detections);
            Console.WriteLine(
                $"Results from Device no. {inputDevice.deviceId} processed successfully. Added {count} detections."
            );
            return count;
        }

        /// <summary>
        /// Delete detection by its ID
        /// </summary>
        /// <param name="recordId">Detection ID</param>
        /// <returns></returns>
        /// <response code="200">The detection was deleted successfully</response>
        /// <response code="204">The detection was not found</response>
        [HttpDelete("api/results/{recordId}")]
        public async Task<IActionResult> DeleteById([FromRoute] string recordId)
        {
            var deletedItems = await _dbOperations.DeleteById(recordId);
            if (deletedItems > 0)
                return Ok("Record deleted successfully");
            else
                return NoContent();
        }
    }
}
