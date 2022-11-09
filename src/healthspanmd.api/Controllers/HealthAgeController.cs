using healthspanmd.api.Attributes;
using healthspanmd.core.Services.HealthAgeCalculator_v2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiKey]
    public class HealthAgeController : ControllerBase
    {

        private readonly IHealthAgeCalculator _healthAgeCalc;

        public HealthAgeController(
            IHealthAgeCalculator healthAgeCalc
            )
        {
            _healthAgeCalc = healthAgeCalc;
        }

        [HttpPost("")]
        public IActionResult CalculateHealthAge([FromBody] HealthAgeCalculatorInputModel model)
        {
            var result = _healthAgeCalc.Execute(model);
            return new JsonResult(result);
        }
    }
}
