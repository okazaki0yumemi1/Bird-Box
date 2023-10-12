using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bird_Box.Data;
using Bird_Box.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bird_Box.Controllers
{
    [ApiExplorerSettings(IgnoreApi=true)]
    public class ResultsController : Controller
    {
        private readonly ILogger<ResultsController> _logger;
        private readonly BirdRepository _dbOperations;

        public ResultsController(ILogger<ResultsController> logger, BirdRepository dbOperations)
        {
            _dbOperations = dbOperations;
            _logger = logger;
        }

        //[Route("~/")]
        public IActionResult Index()
        {
            var details = new AppDetails(_dbOperations);
            return View(details);
        }

        //[Route("~/results")]
        public async Task<IActionResult> ListAll()
        {
            var results = await _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate.Ticks);
            return View("Views/Results/Results.cshtml", results);
        }

        [Route("~/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet("Results/Delete/{objId}")]
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
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Results/Delete/{objId}")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] string objId)
        {
            var deletedItems = await _dbOperations.DeleteById(objId);
            if (deletedItems > 0)
                return Ok("Deleted successfully");//View("Views/Results/Delete.cshtml", deletedItems);
            else
                return NoContent();
            
        }

        [Route("Results/Details/{objId}")]
        public async Task<IActionResult> Details([FromRoute] string objId)
        {
            var bird = await _dbOperations.GetByGuid(objId);
            if (bird == null)
                return NoContent();
            return View("Views/Results/Details.cshtml", bird);
        }

    }
}