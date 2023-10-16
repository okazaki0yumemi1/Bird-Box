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

        [HttpGet("api/results/{recordId}")]
        public async Task<IActionResult> GetByID([FromRoute] string recordId)
        {
            var bird = _dbOperations.GetByGuid(recordId);
            if (bird == null)
                return NotFound();
            return Ok(bird);
        }

        [HttpGet("api/results")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate);
            return Ok(results);
        }

        [HttpGet("api/results/days/{yyyyMMdd}")]
        public async Task<IActionResult> GetAllDetectionsByDay([FromRoute] DateTime yyyyMMdd)
        {
            DateTime date = yyyyMMdd;
            //DateTime.TryParse(yyyyMMdd, out date);
            var results = await _dbOperations.GetByDate(date);
            results.OrderByDescending(x => x.recodingDate.Ticks);
            return Ok(results);
        }

        [HttpGet("api/results/birds/{birdName}")]
        public async Task<IActionResult> GetBirdByName([FromRoute] string birdName)
        {
            var records = _dbOperations.GetByBirdName(birdName);
            if (records == null)
                return NotFound();
            return Ok(records);
        }

        //[HttpPost("api/results/recordings/start/{hours}")]
        //public async Task<IActionResult> StartRecording(
        //    [FromBody] AnalyzerOptions? optionsInput,
        //    [FromRoute] string hours
        //)
        //{
        //    TimeSpan _hours;
        //    var options = new AnalyzerOptions(setWeek: true);
        //    if (optionsInput is null)
        //    {
        //        options = _defaultOptions;
        //    }
        //    else
        //    {
        //        options = ValidModel(optionsInput);
        //        if (options is null)
        //            options = _defaultOptions;
        //    }

        //    if (!TimeSpan.TryParse(hours, out _hours))
        //        _hours = TimeSpan.FromHours(1); //default value - 1 hour
        //    _recordingService.StartRecording(_hours, options);
        //    // Utilities.RecordingSchedule scheduleRecording = new Utilities.RecordingSchedule(_hours);
        //    // ListeningTask = scheduleRecording.RecordAndRecognize(options);
        //    return Ok(
        //        $"The task will be run for {_hours} hours.{Environment.NewLine} The options are:{Environment.NewLine} {JsonSerializer.Serialize(options)}"
        //    );
        //}

        //[HttpGet("api/recordings/status/{serviceId}")]
        //public async Task<bool> RecordingStatus([FromRoute] int serviceId)
        //{
        //    if (_recordingService.RecordingStatus(serviceId) == TaskStatus.Running) return true;
        //    else return false;
        //}

        //[HttpPost("api/recordings/stop/{serviceId}")]
        //public async Task<bool> StopRecording([FromRoute] int serviceId)
        //{
        //    return _recordingService.StopRecording(serviceId);
        //}

        //AnalyzerOptions ValidModel(AnalyzerOptions inputModel)
        //{
        //    var result = new AnalyzerOptions();
        //    if ((inputModel.latitude is not null) && (inputModel.longitude is not null))
        //    {
        //        int la = -1;
        //        int lo = -1;
        //        if (
        //            int.TryParse(inputModel.latitude, out la)
        //            && (int.TryParse(inputModel.longitude, out lo))
        //        )
        //        {
        //            if (((la >= 0) && (lo <= 60)) && ((la >= 0) && (lo <= 60)))
        //            {
        //                result.latitude = la.ToString();
        //                result.longitude = lo.ToString();
        //            }
        //            else
        //            {
        //                result.latitude = "-1";
        //                result.longitude = "-1";
        //            }
        //        }
        //    }
        //    if (inputModel.weekOfTheYear is not null)
        //    {
        //        int w = 0;
        //        if (int.TryParse(inputModel.weekOfTheYear, out w))
        //        {
        //            if ((w >= 1) && (w <= 48))
        //                result.weekOfTheYear = w.ToString();
        //            else if (w == -1)
        //                result.weekOfTheYear = w.ToString();
        //        }
        //        else
        //            result.weekOfTheYear = ((int)DateTime.Now.Day / 7).ToString();
        //    }
        //    if (inputModel.sensitivity is not null)
        //    {
        //        float s = 1;
        //        if (float.TryParse(inputModel.sensitivity, out s))
        //        {
        //            if ((s >= 0.5) || (s <= 1.5))
        //                result.sensitivity = s.ToString();
        //        }
        //    }
        //    if (inputModel.minimumConfidence is not null)
        //    {
        //        float c = 0.01f;
        //        if (float.TryParse(inputModel.minimumConfidence, out c))
        //        {
        //            if ((c >= 0.01) || (c <= 0.99))
        //                result.minimumConfidence = c.ToString();
        //        }
        //    }
        //    if (inputModel.overlapSegments is not null)
        //    {
        //        float oS = 0.0f;
        //        if (float.TryParse(inputModel.sensitivity, out oS))
        //        {
        //            if ((oS >= 0.01) || (oS < 1))
        //                result.sensitivity = oS.ToString();
        //        }
        //    }
        //    if (inputModel.cpuThreads is not null)
        //    {
        //        int t = 1;
        //        if (int.TryParse(inputModel.cpuThreads, out t))
        //        {
        //            if ((t >= 1) || (t < 768))
        //                result.cpuThreads = t.ToString();
        //        }
        //    }
        //    if (inputModel.processingBatchSize is not null)
        //    {
        //        int pBS = 1;
        //        if (int.TryParse(inputModel.sensitivity, out pBS))
        //        {
        //            if ((pBS >= 0.01) || (pBS < 1))
        //                result.sensitivity = pBS.ToString();
        //        }
        //    }
        //    if (inputModel.locale is not null)
        //    {
        //        if (inputModel.Locales.Contains(inputModel.locale))
        //            result.locale = inputModel.locale;
        //    }
        //    if (inputModel.speciesFrequencyThreshold is not null)
        //    {
        //        float sf_thresh = 0.03f;
        //        if (float.TryParse(inputModel.speciesFrequencyThreshold, out sf_thresh))
        //        {
        //            if ((sf_thresh >= 0.01) || (sf_thresh <= 0.99))
        //                result.speciesFrequencyThreshold = sf_thresh.ToString();
        //        }
        //    }
        //    return result;
        //}

        [HttpGet("api/results/process")]
        public async Task<IActionResult> ProcessResults()
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing("Recordings/");
            var results = rrp.ProcessAllFiles();
            return Ok($"Results processed successfully");
        }

        [HttpDelete("api/results/{recordId}")]
        public async Task<IActionResult> DeleteById([FromRoute] string recordId)
        {
            var deletedItems = await _dbOperations.DeleteById(recordId);
            if (deletedItems > 0)
                return Ok("Record deleted successfully");
            else
                return NoContent();
        }

        //[HttpGet("api/recordings/")]
        //public async Task<List<int>> GetAllRunningServices()
        //{
        //    return _recordingService.GetRunningRecordingServices();
        //}
    }
}
