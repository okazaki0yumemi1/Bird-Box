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

    public async Task<IActionResult> Privacy()
    {
        Utilities.RecordingSchedule schedule30sec = new Utilities.RecordingSchedule(1, 30);
        schedule30sec.RecordAndRecognize();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
