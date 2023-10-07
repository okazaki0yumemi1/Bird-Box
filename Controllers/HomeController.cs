using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bird_Box.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bird_Box.Controllers
{
    [ApiExplorerSettings(IgnoreApi=true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BirdRepository _dbOperations;

        public HomeController(ILogger<HomeController> logger, BirdRepository dbOperations)
        {
            _dbOperations = dbOperations;
            _logger = logger;
        }

        [Route("~/")]
        public IActionResult Index()
        {
            return View("Views/Results/Index.cshtml");
        }


        [Route("~/results")]
        public IActionResult Results()
        {
            var results = _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate);
            return View("Views/Results/Results.cshtml", results);
        }

        [Route("~/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [Route("~/delete/{objId}")]
        public IActionResult Delete([FromRoute] string objId)
        {
            var deletedItems = _dbOperations.DeleteById(objId);
            if (deletedItems > 0)
                return View("Views/Results/Delete.cshtml", objId);
            else
                return NoContent();
            
        }
        [Route("~/results/{objId}")]
        public IActionResult Details([FromRoute] string objId)
        {
            var bird = _dbOperations.GetByGuid(objId);
            if (bird == null)
                return NoContent();
            return View("Views/Results/Details.cshtml", bird);
        }

    }
}