using System.Runtime.CompilerServices;
using System.Text.Json;
using Bird_Box.Data;
using Bird_Box.Models;
using Bird_Box.Services;
using Bird_Box.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Bird_Box.Controllers
{
    [ApiController]
    public class ResultsAPIController : ControllerBase
    {
        private readonly BirdRepository _dbOperations;
        //private readonly AnalyzerOptions _defaultOptions;
        //private readonly IConfigurationRoot _config;

        public ResultsAPIController(BirdRepository dbOperations)
        {
            _dbOperations = dbOperations;
            //_config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables()
            //    .Build();

            //// Get values from the config given their key and their target type.
            //_defaultOptions = _config
            //    .GetRequiredSection("BirdNETOptions:Default")
            //    .Get<AnalyzerOptions>();

        }

        /// <summary>
        /// Get detection item by its ID
        /// </summary>
        /// <param name="recordId">Detection ID</param>
        /// <returns></returns>
        [HttpGet("api/results/{recordId}")]
        public async Task<IActionResult> GetByID([FromRoute] string recordId)
        {
            var bird = _dbOperations.GetByGuid(recordId);
            if (bird == null)
                return NotFound();
            return Ok(bird);
        }

        /// <summary>
        /// Get all detections in a local database
        /// </summary>
        /// <returns></returns>
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
        /// <param name="yyyyMMdd">Date in format yyyyMMdd, i.e. 2023-10-12</param>
        /// <returns></returns>
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
        [HttpGet("api/results/birds/{birdName}")]
        public async Task<IActionResult> GetBirdByName([FromRoute] string birdName)
        {
            var records = _dbOperations.GetByBirdName(birdName);
            if (records == null)
                return NotFound();
            return Ok(records);
        }

        /// <summary>
        /// Process all raw results and put them to a database.
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/results/process")]
        public async Task<IActionResult> ProcessResults()
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing("Recordings/");
            var results = rrp.ProcessAllFiles();
            return Ok($"Results processed successfully");
        }

        /// <summary>
        /// Delete detection by its ID
        /// </summary>
        /// <param name="recordId">Detection ID</param>
        /// <returns></returns>
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
