using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public async Task<IActionResult> StartRecording([FromBody] AnalyzerOptions optionsInput, [FromRoute] string hours)
        {
            TimeSpan _hours;
            var options = new AnalyzerOptions();
            options = ValidModel(optionsInput);
            if(!TimeSpan.TryParse(hours, out _hours)) _hours = TimeSpan.FromHours(1); //default value - 1 hour
            Utilities.RecordingSchedule scheduleRecording = new Utilities.RecordingSchedule(_hours);
            ListeningTask = scheduleRecording.RecordAndRecognize(options);
            return Ok($"The task will be run for {_hours} hours.");
        }
        AnalyzerOptions ValidModel(AnalyzerOptions inputModel)
        {
            var result = new AnalyzerOptions();
            if ((inputModel.latitude is not null) && (inputModel.longitude is not null)) 
            {
                int la = -1;
                int lo = -1;
                if(int.TryParse(inputModel.latitude, out la) && (int.TryParse(inputModel.longitude, out lo)))
                {
                    if (((la >= 0) && (lo <= 60)) && ((la >= 0) && (lo <= 60)))
                    {
                        result.latitude = la.ToString();
                        result.longitude = lo.ToString();
                    }
                    else 
                    {
                        result.latitude = "-1";
                        result.longitude = "-1";
                    }
                }
            }
            if (inputModel.weekOfTheYear is not null) 
            {
                int w = 0;
                if (int.TryParse(inputModel.weekOfTheYear, out w))
                {
                    if ((w >= 1) && (w <= 48)) result.weekOfTheYear = w.ToString();
                }
                else result.weekOfTheYear = "-1";
            }
            if (inputModel.sensitivity is not null) 
            {
                float s = 1;
                if(float.TryParse(inputModel.sensitivity, out s))
                {
                    if ((s >= 0.5) || (s <= 1.5)) result.sensitivity = s.ToString();
                }
            }
            if (inputModel.minimumConfidence is not null) 
            {
                float c = 0.01f;
                if(float.TryParse(inputModel.minimumConfidence, out c))
                {
                    if ((c >= 0.01) || (c <= 0.99)) result.minimumConfidence = c.ToString();
                }
            }
            if (inputModel.overlapSegments is not null)
            {
                float oS = 0.0f;
                if(float.TryParse(inputModel.sensitivity, out oS))
                {
                    if ((oS >= 0.01) || (oS < 1)) result.sensitivity = oS.ToString();
                }
            }
            if (inputModel.overlapSegments is not null)
            {
                int t = 1;
                if(int.TryParse(inputModel.sensitivity, out t))
                {
                    if ((t >= 1) || (t < 768)) result.sensitivity = t.ToString();
                }
            }
            if (inputModel.processingBatchSize is not null)
            {
                int pBS = 1;
                if(int.TryParse(inputModel.sensitivity, out pBS))
                {
                    if ((pBS >= 0.01) || (pBS < 1)) result.sensitivity = pBS.ToString();
                }
            }
            if (inputModel.locale is not null)
            {
                if (inputModel.Locales.Contains(inputModel.locale)) result.locale = inputModel.locale;
            }
            if (inputModel.locale is not null)
            {
                float sf_thresh = 0.03f;
                if(float.TryParse(inputModel.sensitivity, out sf_thresh))
                {
                    if ((sf_thresh >= 0.01) || (sf_thresh <= 0.99)) result.sensitivity = sf_thresh.ToString();
                }
            }
            return result;
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