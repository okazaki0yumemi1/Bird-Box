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
        public async Task<IActionResult> Results()
        {
            var results = await _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate);
            return View("Views/Results/Results.cshtml", results);
        }

        [Route("~/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        public async Task<IActionResult> Delete([FromRoute] string objId)
        {

            if (objId == null)
            {
                return NotFound();
            }

            var bird = await _dbOperations.GetByGuid(objId);
            if (bird == null)
            {
                return NotFound();
            }

            return View(bird);
        }

        [Route("~/delete/{objId}")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] string objId)
        {
            var deletedItems = await _dbOperations.DeleteById(objId);
            if (deletedItems > 0)
                return View("Views/Results/Delete.cshtml", objId);
            else
                return NoContent();
            
        }
        [Route("~/results/{objId}")]
        public async Task<IActionResult> Details([FromRoute] string objId)
        {
            var bird = await _dbOperations.GetByGuid(objId);
            if (bird == null)
                return NoContent();
            return View("Views/Results/Details.cshtml", bird);
        }

    }
}