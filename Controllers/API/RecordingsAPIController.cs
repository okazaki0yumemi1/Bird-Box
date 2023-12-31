using System.Globalization;
using System.Text.Json;
using Bird_Box.Audio;
using Bird_Box.Data;
using Bird_Box.Models;
using Bird_Box.Services;
using Bird_Box.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bird_Box.Controllers
{
    [ApiController]
     public class RecordingsAPIController : ControllerBase
     {
        private RecordingService _recordingService;
        private readonly AnalyzerOptions _defaultOptions;
        private readonly IConfigurationRoot _config;
        private readonly MicrophoneRepository _microphoneContext;

        public RecordingsAPIController(RecordingService recordingService, MicrophoneRepository microphoneContext)
        {
            _recordingService = recordingService;
            _microphoneContext = microphoneContext;
            // Get values from the config given their key and their target type.
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            _defaultOptions = _config
                .GetRequiredSection("BirdNETOptions:Default")
                .Get<AnalyzerOptions>();
        }

        /// <summary>
        /// Start listening service by passing a BirdNET settings and a length (in hours)
        /// </summary>
        /// <param name="optionsInput">BirdNET Analyzer settings</param>
        /// <param name="hours">Duration, hours</param>
        /// <param name="inputDevice">Input device ID</param>
        /// <returns></returns>
        [HttpPost("api/results/recordings/start/{hours}")]
        public async Task<IActionResult> StartRecording(
            [FromBody] AnalyzerOptions? optionsInput,
            [FromRoute] string inputDevice,
            [FromQuery] string? hours
        )
        {
            if (inputDevice is null || inputDevice == string.Empty)
            return BadRequest("No input device provided.");
            
            Microphone? device;
            if (MicrophoneExist(inputDevice))
            {
                var inputDevices = CommandLine.GetAudioDevices();
                device = inputDevices.FirstOrDefault(x => x.deviceId == "hw:" + inputDevice);
                if (device == null) return BadRequest($"Can't find input device with provided id: {inputDevice}");
                _microphoneContext.Create(device);
                //if the input device is new, i.e. not in a database, then add it no a DB.
                //otherwise, add default device;
                // if (device is not null)
                // {
                //     if (await _dbOperations.GetById(device.deviceId) is null)
                //     {
                //         await _dbOperations.Create(device);
                //         Console.WriteLine($"New input device with ID={device.deviceId}, Info = {device.deviceInfo} and database ID={device.objId} was added to a database.");
                //     }
                // }
                // else if (await _dbOperations.GetById("-1") is null)
                // {
                //     await _dbOperations.Create(new Microphone("-1", "Default device"));
                //     Console.WriteLine($"Added default input device with ID = \"-1\".");
                // }
            }
            else return BadRequest($"Can't find input device with provided id: {inputDevice}");

            TimeSpan _hours;
            var options = new AnalyzerOptions(setWeek: true);
            if (optionsInput is null)
            {
                options = _defaultOptions;
            }
            else
            {
                options = ValidModel(optionsInput);
                if (options is null)
                    options = _defaultOptions;
            }

            if (!TimeSpan.TryParse(hours, out _hours))
                _hours = TimeSpan.FromHours(1); //default value - 1 hour
            Console.WriteLine($"Using input device ID={device.deviceId}...");
            _recordingService.StartRecording(_hours, options, device.deviceId); //using deviceId
            
            return Ok(
                $"The task will be run for {_hours} hours.{Environment.NewLine} The options are:{Environment.NewLine} {JsonSerializer.Serialize(options)}"
            );
        }

        /// <summary>
        /// Get status of a listening service.
        /// </summary>
        /// <param name="serviceId">Listening service ID</param>
        /// <returns>True if task is running, othewise return false</returns>
        [HttpGet("api/recordings/status/{serviceId}")]
        public async Task<bool> RecordingStatus([FromRoute] int serviceId)
        {
            if (_recordingService.RecordingStatus(serviceId) == TaskStatus.Running)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Stop listening service by its ID
        /// </summary>
        /// <param name="serviceId">Listening service ID</param>
        /// <returns></returns>
        [HttpGet("api/recordings/stop/{serviceId}")]
        public async Task<bool> StopRecording([FromRoute] int serviceId)
        {
            return await _recordingService.StopRecording(serviceId);
        }

        /// <summary>
        /// Get all running listening services
        /// </summary>
        /// <returns>List of IDs</returns>
        [HttpGet("api/recordings/")]
        public async Task<List<int>> GetAllRunningServices()
        {
            return _recordingService.GetRunningRecordingServices();
        }

        /// <summary>
        /// Get all connected and available input devices to a computer
        /// </summary>
        /// <returns>List of input devices</returns>
        [HttpGet("api/recordings/microphones")]
        public List<Microphone> GetAllConnectedInputDevices()
        {
            var devices = _recordingService.GetInputDevices();
            return devices;
        }

        /// <summary>
        /// Validate neural network settings
        /// </summary>
        /// <param name="inputModel">BirdNET Analyzer settings</param>
        /// <returns>Correct settings class</returns>
        private AnalyzerOptions ValidModel(AnalyzerOptions inputModel)
        {
            var result = new AnalyzerOptions();
            if ((inputModel.latitude is not null) && (inputModel.longitude is not null))
            {
                int la = -1;
                int lo = -1;
                if (
                    int.TryParse(inputModel.latitude, out la)
                    && (int.TryParse(inputModel.longitude, out lo))
                )
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
                    if ((w >= 1) && (w <= 48))
                        result.weekOfTheYear = w.ToString();
                    else if (w == -1)
                        result.weekOfTheYear = w.ToString();
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    Calendar cal = CultureInfo.CurrentCulture.Calendar;
                    w = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                }
                result.weekOfTheYear = w.ToString();
            }
            if (inputModel.sensitivity is not null)
            {
                float s = 1;
                if (float.TryParse(inputModel.sensitivity, out s))
                {
                    if ((s >= 0.5) || (s <= 1.5))
                        result.sensitivity = s.ToString();
                }
            }
            if (inputModel.minimumConfidence is not null)
            {
                float c = 0.01f;
                if (float.TryParse(inputModel.minimumConfidence, out c))
                {
                    if ((c >= 0.01) || (c <= 0.99))
                        result.minimumConfidence = c.ToString();
                }
            }
            if (inputModel.overlapSegments is not null)
            {
                float oS = 0.0f;
                if (float.TryParse(inputModel.sensitivity, out oS))
                {
                    if ((oS >= 0.01) || (oS < 1))
                        result.sensitivity = oS.ToString();
                }
            }
            if (inputModel.cpuThreads is not null)
            {
                int t = 1;
                if (int.TryParse(inputModel.cpuThreads, out t))
                {
                    if ((t >= 1) || (t < 768))
                        result.cpuThreads = t.ToString();
                }
            }
            if (inputModel.processingBatchSize is not null)
            {
                int pBS = 1;
                if (int.TryParse(inputModel.sensitivity, out pBS))
                {
                    if ((pBS >= 0.01) || (pBS < 1))
                        result.sensitivity = pBS.ToString();
                }
            }
            if (inputModel.locale is not null)
            {
                if (inputModel.Locales.Contains(inputModel.locale))
                    result.locale = inputModel.locale;
            }
            if (inputModel.speciesFrequencyThreshold is not null)
            {
                float sf_thresh = 0.03f;
                if (float.TryParse(inputModel.speciesFrequencyThreshold, out sf_thresh))
                {
                    if ((sf_thresh >= 0.01) || (sf_thresh <= 0.99))
                        result.speciesFrequencyThreshold = sf_thresh.ToString();
                }
            }
            return result;
        }
        private bool MicrophoneExist(string deviceId)
        {
            var device = CommandLine.GetAudioDevices().FirstOrDefault(x => x.deviceId == deviceId);
            if (device is null) return false;
            else return true;
        }
    }
}
