using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bird_Box.Data;
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
        [HttpPost("results/start/{confidenceThreshold}")]
        public async Task<IActionResult> StartRecording([FromRoute] string? confidenceThreshold, [FromBody] string recordingTimeInHours)
        {
            if (!ListeningTask.IsCompleted) return Conflict("Listening task is already running!");
            TimeSpan hours;
            if(!TimeSpan.TryParse(recordingTimeInHours, out hours)) hours = TimeSpan.FromHours(1); //default value - 1 hour
            Utilities.RecordingSchedule scheduleRecording = new Utilities.RecordingSchedule(hours);
            ListeningTask = scheduleRecording.RecordAndRecognize(confidenceThreshold);
            return Ok($"The task will be run for {hours} hours.");
        }
        [HttpGet("results/process")]
        public async Task<IActionResult> WriteToDB()
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing("Recordings/");
            var results = rrp.ProcessAllFiles();
            return Ok($"Added {_dbOperations.CreateRange(results)} results");
        }
    }
}
/*
        Utilities.RecordingSchedule schedule30sec = new Utilities.RecordingSchedule(TimeSpan.FromHours(3));
        Task listening = schedule30sec.RecordAndRecognize();
        Task.WaitAll(listening);
*/