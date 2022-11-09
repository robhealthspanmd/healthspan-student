using healthspanmd.api.Attributes;
using healthspanmd.core.CQRS.Metrics;
//using healthspanmd.core.CQRS.MetricTypes;
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
    public class MetricController : Controller
    {
        private readonly IMetricCommands _metricCommands;
        //private readonly IMetricTypeCommands _metricTypeCommands;
        private readonly IMetricQueries _metricQueries;


        public MetricController (
            IMetricCommands metricCommands,
            //IMetricTypeCommands metricTypeCommands,
            IMetricQueries metricQueries
            )
        {
            _metricCommands = metricCommands;
            //_metricTypeCommands = metricTypeCommands;
            _metricQueries = metricQueries;
        }

     
        [HttpGet("")]
        public IActionResult GetMetrics()
        {
            var response = _metricQueries.GetMetrics(new GetMetricsQueryFilter());
            return new JsonResult(response);
        }

        [HttpPost("")]
        public IActionResult CreateMetric([FromBody] MetricModel model)
        {
            var response = _metricCommands.CreateOrUpdate(model);
            return new JsonResult(response);
        }


        //[HttpPost("Type")]
        //public IActionResult CreateMetricType([FromBody] MetricTypeModel model)
        //{
        //    var response = _metricTypeCommands.Create(model);
        //    return new JsonResult(response);
        //}


    }
}
