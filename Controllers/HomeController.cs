using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bird_Box.Models;

namespace Bird_Box.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        Utilities.CommandLine bash = new Utilities.CommandLine();
        Audio.FFMpegSettings newSettings = new Audio.FFMpegSettings();
        var inputDevices = bash.GetAudioDevices();
        Audio.Recording recordingObj = new Audio.Recording(inputDevices.Where(x => x.deviceInfo.Contains("USB")).First(), newSettings);
        var file = recordingObj.RecordAudio();
        Audio.AudioProcessing audio = new Audio.AudioProcessing("/Recordings");
        audio.ProcessAudio(file);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
