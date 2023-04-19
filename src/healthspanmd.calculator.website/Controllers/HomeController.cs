using healthspanmd.calculator.website.Models;
using healthspanmd.core.Services.HealthAgeCalculator_v3;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace healthspanmd.calculator.website.Controllers
{
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> HealthAge(Patient patient)
        {
            // Do something with the patient object
            HealthAgeCalculator cal = new HealthAgeCalculator();
            HealthAgeCalculatorResult result = await cal.CalculateResultAsync(patient);
            HealthAgeFinal healthAgeFinal = new HealthAgeFinal();
            healthAgeFinal.result = result;
            healthAgeFinal.patient = patient;

            return View(healthAgeFinal);
        }
    }
}