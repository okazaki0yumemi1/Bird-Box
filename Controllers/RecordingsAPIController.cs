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
	public class RecordingsAPIController : ControllerBase
	{
		private RecordingService _recordingService;
		private readonly AnalyzerOptions _defaultOptions;
		private readonly IConfigurationRoot _config;

		public RecordingsAPIController(RecordingService recordingService)
		{
			_recordingService = recordingService;

			// Get values from the config given their key and their target type.
			_defaultOptions = _config
				.GetRequiredSection("BirdNETOptions:Default")
				.Get<AnalyzerOptions>();
		}

		[HttpPost("api/results/recordings/start/{hours}")]
		public async Task<IActionResult> StartRecording(
			[FromBody] AnalyzerOptions? optionsInput,
			[FromRoute] string hours
		)
		{
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
			_recordingService.StartRecording(_hours, options);
			return Ok(
				$"The task will be run for {_hours} hours.{Environment.NewLine} The options are:{Environment.NewLine} {JsonSerializer.Serialize(options)}"
			);
		}

		[HttpGet("api/recordings/status/{serviceId}")]
		public async Task<bool> RecordingStatus([FromRoute] int serviceId)
		{
			if (_recordingService.RecordingStatus(serviceId) == TaskStatus.Running) return true;
			else return false;
		}

		[HttpGet("api/recordings/stop/{serviceId}")]
		public async Task<bool> StopRecording([FromRoute] int serviceId)
		{
			return _recordingService.StopRecording(serviceId);
		}

		[HttpGet("api/recordings/")]
		public async Task<List<int>> GetAllRunningServices()
		{
			return _recordingService.GetRunningRecordingServices();
		}

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
				//{
				//                DateTime dt = DateTime.Now;
				//                Calendar cal = new CultureInfo.GetCurrentCulture().Calendar;
				//                int week = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
				//}
					result.weekOfTheYear = ((int)DateTime.Now.Day / 7).ToString();
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
	}
}