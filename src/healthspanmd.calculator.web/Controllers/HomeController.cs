using healthspanmd.calculator.web.Models;
using healthspanmd.core.Services.HealthAgeCalculator_v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.calculator.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHealthAgeCalculator _healthAgeCalculator;

        public HomeController(
            ILogger<HomeController> logger,
            IHealthAgeCalculator healthAgeCalculator
            )
        {
            _logger = logger;
            _healthAgeCalculator = healthAgeCalculator;
        }

        [HttpGet]
        [Route("")]
        [Route("/Index")]
        public IActionResult Index()
        {
            var model = new CalculatorInputResultViewModel()
            {
                Input = new HealthAgeCalculatorInputModel(),
                Result = new CalculatorResultModel()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CalculateAge(CalculatorInputResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _healthAgeCalculator.Execute(model.Input);
                model.Result = new CalculatorResultModel()
                {
                    HealthAge = result.HealthAge
                };
                return View("Index", model);
            }
            else
            {
                return View(model);
            }
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
    }
}
