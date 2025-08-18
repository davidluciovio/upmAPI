using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using upmDomain.Lider;

namespace upmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiderController : Controller
    {
        private readonly LiderService _liderService;
        public LiderController(LiderService liderService)
        {
            _liderService = liderService;
        }
        // GET: LiderController
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var response = await _liderService.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno en el servidor: {ex.Message}, {ex.InnerException?.Message}");
            }
        }

        // GET: LiderController/Details/5
        [HttpGet("{id:int}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LiderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LiderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LiderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LiderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
