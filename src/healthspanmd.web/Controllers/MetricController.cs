using healthspanmd.core.CQRS.Metrics;
using healthspanmd.shared.Extensions;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models.Metric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class MetricController : Controller
    {
        private readonly IMetricQueries _metricQueries;
        private readonly IMetricCommands _metricCommands;

        public MetricController (
            IMetricQueries metricQueries,
            IMetricCommands metricCommands
            )
        {
            _metricQueries = metricQueries;
            _metricCommands = metricCommands;
        }


        [Authorize(Policy = "CanEditMetrics")]
        [HttpGet]
        [Route("Metric")]
        public IActionResult Index()
        {
            this.AddBreadCrumb("Metrics");

            var model = GetAdminMetricListViewModel();
            
            return View(model);
        }


        private MetricListViewModel GetAdminMetricListViewModel()
        {
            var metrics = _metricQueries.GetMetrics(new GetMetricsQueryFilter(), true);

            var model = new MetricListViewModel
            {
                Items = metrics.Select(m => new MetricListViewModel.MetricItem
                {
                    MetricId = m.MetricId,
                    Name = m.Name,
                    Description = m.Description,
                    MetricType = m.DataType.ToString(),
                    UsersCount = m.UserCountWithThisMetricAsActive,
                    IsActive = m.IsActive
                }).ToList()
            };

            return model;
        }

        [Authorize(Policy = "CanEditMetrics")]
        [HttpPost]
        [Route("Metric/Update")]
        public async Task<IActionResult> UpdateMetricAsync([FromBody] MetricViewModel metric)
        {
            // update existing or create new metric
            var metricModel = metric.ToMetricModel();
            if (metricModel.SelectItems != null)
                foreach (var selectItemModel in metricModel.SelectItems)
                    if (selectItemModel.MetricSelectItemId < 0)
                        selectItemModel.MetricSelectItemId = 0;
            var result = _metricCommands.CreateOrUpdate(metricModel);

            bool success = result.Success;
          
            return Json(new { success });
        }


        [Authorize(Policy = "CanEditMetrics")]
        [HttpGet]
        [Route("Metric/Deactivate/{metricId}")]
        public async Task<IActionResult> DeactivateMetricAsync(int metricId)
        {
            var result = _metricCommands.SetActiveStatus(metricId, false);
            bool success = result.Success;
            string html = "";

            if (result.Success)
            {
                // get fresh list of metrics and return partial view
                var model = GetAdminMetricListViewModel();
                html = await this.RenderViewAsync("/Views/Metric/_AdminMetricList.cshtml", model, true);
            }

            return Json(new { success, html });
        }



        [Authorize(Policy = "CanEditMetrics")]
        [HttpGet]
        [Route("Metric/Delete/{metricId}")]
        public async Task<IActionResult> DeleteMetricAsync(int metricId)
        {
            var result = _metricCommands.Delete(metricId);
            bool success = result.Success;
            string html = "";

            if (result.Success)
            {
                // get fresh list of metrics and return partial view
                var model = GetAdminMetricListViewModel();
                html = await this.RenderViewAsync("/Views/Metric/_AdminMetricList.cshtml", model, true);
            }

            return Json(new { success, html });
        }


        [Authorize(Policy = "CanEditMetrics")]
        [HttpGet]
        [Route("Metric/Activate/{metricId}")]
        public async Task<IActionResult> ActivateMetricAsync(int metricId)
        {
            var result = _metricCommands.SetActiveStatus(metricId, true);
            bool success = result.Success;
            string html = "";

            if (result.Success)
            {
                // get fresh list of metrics and return partial view
                var model = GetAdminMetricListViewModel();
                html = await this.RenderViewAsync("/Views/Metric/_AdminMetricList.cshtml", model, true);
            }

            return Json(new { success, html });
        }

        //[HttpGet]
        //[Route("Metric/Model/{metricId}")]
        //public IActionResult GetMetricViewModel(int metricId)
        //{
        //    var model = _metricQueries.GetMetric(metricId);
        //    var metric = model.ToMetricViewModel();
        //    return Json(new { success = true, metric });
        //}


        [Authorize(Policy = "CanEditMetrics")]
        [HttpGet]
        [Route("Metric/Detail/{metricId}")]
        public IActionResult AdminMetricDetailForm(int metricId)
        {
            this.AddBreadCrumb("Metrics","/Metric");
            if (metricId == 0)
                this.AddBreadCrumb("Create Metric");
            else
                this.AddBreadCrumb("Edit Metric");


            MetricViewModel model = null;
            if (metricId == 0)
            {
                model = new MetricViewModel 
                { 
                    SelectItems = new System.Collections.Generic.List<MetricSelectItemViewModel>() ,
                    DataType = -1,
                    IsActive = true
                };
            }
            else
            {
                var metricModel = _metricQueries.GetMetric(metricId);
                model = metricModel.ToMetricViewModel();
            }          
            return View(model);
        }
    }
}
