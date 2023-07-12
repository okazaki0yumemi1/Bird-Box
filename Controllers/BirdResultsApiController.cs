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
            return Ok(results);
        }
        [HttpGet("results/{birdName}")]
        public async Task<IActionResult> GetBirdByName([FromRoute] string birdName)
        {
            var records = _dbOperations.GetByBirdName(birdName);
            if (records == null) return NotFound();
            return Ok(records);
        }
        [HttpPost("results/start/{hours}")]
        public async Task<IActionResult> StartRecordingForHours([FromRoute]double hours)
        {
            Utilities.RecordingSchedule schedule30sec = new Utilities.RecordingSchedule(TimeSpan.FromHours(hours));
            ListeningTask = schedule30sec.RecordAndRecognize();
            return Ok();
        }
        [HttpGet("results/process")]
        public async Task<IActionResult> WriteToDB()
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing("Recordings/");
            var results = rrp.ProcessAllFiles();
            _dbOperations.CreateRange(results);
            return Ok();
        }
    }
}
/*
        Utilities.RecordingSchedule schedule30sec = new Utilities.RecordingSchedule(TimeSpan.FromHours(3));
        Task listening = schedule30sec.RecordAndRecognize();
        Task.WaitAll(listening);
*/