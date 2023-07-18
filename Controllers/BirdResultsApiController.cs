using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bird_Box.Data;
using Bird_Box.Models;
using Bird_Box.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Bird_Box.Controllers
{
    [ApiController]
    [Route("api/")]
    public class BirdResultsApiController : ControllerBase
    {
        Task ListeningTask;
        private readonly BirdRepository _dbOperations;
        public BirdResultsApiController(BirdRepository dbOperations)
        {
            _dbOperations = dbOperations;
        }

        [HttpGet("results/{recordId}")]
        public async Task<IActionResult> GetByID([FromRoute] string recordId)
        {
            var bird = _dbOperations.GetByGuid(recordId);
            if (bird == null) return NotFound();
            return Ok(bird);
        }
        [HttpGet("results/")]
        public async Task<IActionResult> GetAll()
        {
            var results = _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate);
            return Ok(results);
        }
        [HttpPost("results/byDate")]
        public async Task<IActionResult> GetAllDetectionsByDay([FromBody] DateTime yyyyMMdd)
        {
            DateTime date = yyyyMMdd;
            //DateTime.TryParse(yyyyMMdd, out date);
            var results = _dbOperations.GetByDate(date);
            results.OrderByDescending(x => x.recodingDate);
            return Ok(results);
        }
        [HttpGet("results/bird/{birdName}")]
        public async Task<IActionResult> GetBirdByName([FromRoute] string birdName)
        {
            var records = _dbOperations.GetByBirdName(birdName);
            if (records == null) return NotFound();
            return Ok(records);
        }
        [HttpPost("results/start/{hours}")]
        public async Task<IActionResult> StartRecording([FromBody] AnalyzerOptions options, [FromRoute] string hours)
        {
            TimeSpan _hours;
            if(!TimeSpan.TryParse(hours, out _hours)) _hours = TimeSpan.FromHours(1); //default value - 1 hour
            Utilities.RecordingSchedule scheduleRecording = new Utilities.RecordingSchedule(_hours);
            ListeningTask = scheduleRecording.RecordAndRecognize(options);
            return Ok($"The task will be run for {_hours} hours.");
        }
        [HttpGet("results/process")]
        public async Task<IActionResult> WriteToDB()
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing("Recordings/");
            var results = rrp.ProcessAllFiles();
            return Ok($"Added {_dbOperations.CreateRange(results)} results");
        }
        [HttpDelete("results/delete/{recordId}")]
        public async Task<IActionResult> DeleteById([FromRoute] string recordId)
        {
            var deletedItems = _dbOperations.DeleteById(recordId);
            if (deletedItems > 0) return Ok("Record deleted successfully");
            else return NoContent();
        }
    }
}