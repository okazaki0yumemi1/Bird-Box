using System.Text.Json;
using System.Text.Json.Serialization;
using Bird_Box.Data;
using Bird_Box.Models;
using Bird_Box.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Bird_Box.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OptionsController : ControllerBase
    {
        IConfigurationRoot _config;
        AnalyzerOptions? _options;
        public OptionsController()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            // Get values from the config given their key and their target type.
            _options = _config.GetRequiredSection("BirdNETOptions:Default").Get<AnalyzerOptions>();
        }
        [HttpGet]
        public AnalyzerOptions? GetBirdNETOptions()
        {
            return _options;
        }
        AnalyzerOptions ValidModel(AnalyzerOptions inputModel)
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
