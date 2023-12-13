using Bird_Box.Data;
using Bird_Box.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bird_Box.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ResultsController : Controller
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly ILogger<ResultsController> _logger;
        private readonly BirdRepository _dbOperations;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ResultsController(ILogger<ResultsController> logger, BirdRepository dbOperations)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _dbOperations = dbOperations;
            _logger = logger;
        }

        //[Route("~/")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActionResult Index()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var details = new AppDetails(_dbOperations);
            return View(details);
        }

        //[Route("~/results")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IActionResult> ListAll()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var results = await _dbOperations.GetAll();
            results.OrderByDescending(x => x.recodingDate.Ticks);
            return View("Views/Results/Results.cshtml", results);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IActionResult> RedirectToSwagger()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return RedirectPermanent("~/api/swagger");
        }

        [Route("~/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActionResult Error()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return View("Error!");
        }

        [HttpGet("Results/Delete/{objId}")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IActionResult> Delete([FromRoute] string objId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (objId == null)
            {
                return NotFound();
            }

            var bird = await _dbOperations.GetById(objId);
            if (bird == null)
            {
                return NotFound();
            }

            return View(bird);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Results/Delete/{objId}")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IActionResult> DeleteConfirmed([FromRoute] string objId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var deletedItems = await _dbOperations.DeleteById(objId);
            if (deletedItems > 0)
                return Ok("Deleted successfully"); //View("Views/Results/Delete.cshtml", deletedItems);
            else
                return NoContent();
        }

        [Route("Results/Details/{objId}")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IActionResult> Details([FromRoute] string objId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var bird = await _dbOperations.GetById(objId);
            if (bird == null)
                return NoContent();
            return View("Views/Results/Details.cshtml", bird);
        }
    }
}
