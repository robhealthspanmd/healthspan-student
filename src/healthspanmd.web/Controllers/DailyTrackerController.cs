using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Metrics;
using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.ExceptionReporting;
using healthspanmd.core.Services.FileSystem;
using healthspanmd.core.Services.SMS;
using healthspanmd.shared.Extensions;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models.Content;
using healthspanmd.web.Models.DailyTracker;
using healthspanmd.web.Models.Metric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class DailyTrackerController : Controller
    {
        private readonly IUserQueries _userQueries;
        private readonly IExceptionReporter _expectionReporter;
        private readonly IMetricTrackingEntryCommands _trackingEntryCommands;
        private readonly IMetricQueries _metricQueries;
        private readonly IMetricTrackingEntryQueries _trackingEntryQueries;
        private readonly IContentQueries _contentQueries;
        private readonly IFileSystemManager _fileSystemManager;

        public DailyTrackerController(
            IUserQueries userQueries,
            IExceptionReporter expectionReporter,
            IMetricTrackingEntryCommands trackingEntryCommands,
            IMetricQueries metricQueries,
            IMetricTrackingEntryQueries trackingEntryQueries,
            IContentQueries contentQueries,
            IFileSystemManager fileSystemManager
            )
        {
            _userQueries = userQueries;
            _expectionReporter = expectionReporter;
            _trackingEntryCommands = trackingEntryCommands;
            _metricQueries = metricQueries;
            _trackingEntryQueries = trackingEntryQueries;
            _contentQueries = contentQueries;
            _fileSystemManager = fileSystemManager;
        }

        [HttpGet]
        [Route("/DailyTracker")]
        [Route("/DailyTracker/Index")]
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var user = _userQueries.GetUserDetailModel(userName, false);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);
            var userLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);
            var localDate = userLocalTime.Date;
            this.AddBreadCrumb("Daily Tracker");
            var model = GetDailyTrackerViewModel(localDate);
            return View(model);
        }

        [HttpGet]
        [Route("/DailyTracker/{dateStr}")]
        public IActionResult Index(string dateStr)
        {
            this.AddBreadCrumb("Daily Tracker");
            var model = GetDailyTrackerViewModel(dateStr.JavascriptDateStringToDate());
            
            return View(model);
        }


        [HttpGet]
        [Route("/DailyTracker/GetDailyTrackerForDay/{dateStr}/{editMode?}")]
        public async Task<IActionResult> GetDailyTrackerForDayAsync(string dateStr, string editMode = "noedit")
        {
            try
            {
                var model = GetDailyTrackerViewModel(dateStr.JavascriptDateStringToDate());
                model.EditMode = editMode == "edit";
                var html = await this.RenderViewAsync("/Views/DailyTracker/_DailyTracker.cshtml", model, true);
                var success = true;
                var metrics = model.MetricsJson;
                return Json(new { success, html, metrics });
            }
            catch (Exception ex)
            {
                _expectionReporter.Execute(ex, this.GetType());
                return Json(new { success = false });
            }
            
        }


        [HttpGet]
        [Route("/DailyTracker/GetMetricsFoundForDay/{dateStr}")]
        public IActionResult GetMetricsFoundForDay(string dateStr)
        {
            try
            {
                var model = GetDailyTrackerViewModel(dateStr.JavascriptDateStringToDate());
                return Json(new { success = true, foundMetrics = model.ExistingTrackingFound });
            }
            catch (Exception ex)
            {
                _expectionReporter.Execute(ex, this.GetType());
                return Json(new { success = false });
            }
        }


        private DailyTrackerViewModel GetDailyTrackerViewModel(DateTime trackingDate)
        {
            var userName = User.Identity.Name;
            var user = _userQueries.GetUserDetailModel(userName, false);

            var metricsCollection = user.ActiveMetrics
                .Where(m => m.Frequency == MetricFrequencyType.Daily || 
                                (m.Frequency == MetricFrequencyType.Weekly && m.DayOfWeek == trackingDate.DayOfWeek))
                .Select(m => m.Metric.ToMetricViewModel()).ToList();

            var trackingData = _trackingEntryQueries.GetList(new GetMetricTrackingEntriesQueryFilter
            {
                UserId = user.UserId,
                StartDate = trackingDate,
                EndDate = trackingDate,
            });

            var model = new DailyTrackerViewModel
            {
                Metrics = metricsCollection,
                MetricsJson = JsonSerializer.Serialize(metricsCollection),
                TrackingDate = trackingDate,
                TrackingData = trackingData,
                ContentCardPreviews = new List<ContentCardPreviewViewModel>()
            };

            foreach (var contentCardAssignment in user.ContentCardAssignments.Where(a => !a.CompletedUtc.HasValue).OrderBy(a => a.SortOrder))
            {
                var contentCardPreview = new ContentCardPreviewViewModel
                {
                    ContentCardId = contentCardAssignment.ContentCardId,
                    Name = contentCardAssignment.ContentCard.Name,
                    Description = contentCardAssignment.ContentCard.Description
                };
                if (contentCardAssignment.ContentCard.ImageFileId.HasValue)
                {
                    var imageContentFile = _contentQueries.GetContentFile(contentCardAssignment.ContentCard.ImageFileId.Value);
                    var imageData = _fileSystemManager.GetFile("contentfiles", imageContentFile.FileSystemPath);
                    var imageBase64Data = Convert.ToBase64String(imageData);
                    contentCardPreview.CardImageSrc = $"data:image/{imageContentFile.FileExtension};base64,{imageBase64Data}";
                }
                model.ContentCardPreviews.Add(contentCardPreview);
            }


            return model;
        }


        [HttpPost]
        [Route("/DailyTracker/Submit")]
        public IActionResult Submit([FromBody] DailyTrackerSubmissionViewModel model)
        {
            try
            {
                var requestModel = GetAddMetricTrackingEntryRequest(model); 

                var response = _trackingEntryCommands.CreateOrReplaceEntries(requestModel);
                if (response.Success)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });
            }
            catch (Exception ex)
            {
                _expectionReporter.Execute(ex, this.GetType());
                return Json(new { success = false });
            }
           
        }

        private AddMetricTrackingEntryRequest GetAddMetricTrackingEntryRequest(DailyTrackerSubmissionViewModel model)
        {
            var entryForDate = Convert.ToDateTime(model.TrackingDataForDateStr);
            var metricLibrary = _metricQueries.GetMetrics(new GetMetricsQueryFilter());
            var user = _userQueries.GetUserDetailModel(HttpContext.User.Identity.Name, false);

            var requestModel = new AddMetricTrackingEntryRequest
            {
                TrackingItems = new List<AddMetricTrackingEntryRequestItem>()
            };

            foreach (var trackedMetric in model.TrackingData)
            {
                var metricDef = metricLibrary.Where(m => m.MetricId == trackedMetric.MetricId).FirstOrDefault();
                var trackingItem = new AddMetricTrackingEntryRequestItem
                {
                    MetricId = trackedMetric.MetricId,
                    EntryDateUtc = DateTime.UtcNow,
                    EntryForDate = entryForDate,
                    UserId = user.UserId
                };

                switch (metricDef.DataType)
                {
                    case MetricDataType.Alpha:
                    case MetricDataType.Alpha_Select:
                        trackingItem.EntryValues = trackedMetric.Values.Select(tm => new MetricTrackingEntryValueModel()
                        {
                            ValueAsString = tm
                        }).ToList();
                        break;

                    case MetricDataType.Numeric_Decimal:
                    case MetricDataType.Numeric_Integer:
                    case MetricDataType.Numeric_Integer_Dial:
                    case MetricDataType.Numeric_Integer_Slider:
                        trackingItem.EntryValues = trackedMetric.Values.Select(tm => new MetricTrackingEntryValueModel()
                        {
                            ValueAsNumber = Convert.ToDouble(tm)
                        }).ToList();
                        break;

                    case MetricDataType.Numeric_Integer_2Values:
                        // should be 2 values
                        trackingItem.EntryValues = new List<MetricTrackingEntryValueModel>()
                        {
                            new MetricTrackingEntryValueModel
                            {
                                ValueAsNumber = Convert.ToDouble(trackedMetric.Values.FirstOrDefault()),
                                Value2AsNumber = Convert.ToDouble(trackedMetric.Values.LastOrDefault())
                            }
                        };
                        break;


                    case MetricDataType.Date:
                        trackingItem.EntryValues = trackedMetric.Values.Select(tm => new MetricTrackingEntryValueModel()
                        {
                            ValueAsDate = Convert.ToDateTime(tm)
                        }).ToList();
                        break;

                    case MetricDataType.YesNo:
                        trackingItem.EntryValues = trackedMetric.Values.Select(tm => new MetricTrackingEntryValueModel()
                        {
                            ValueAsBoolean = Convert.ToBoolean(tm)
                        }).ToList();
                        break;
                }
                requestModel.TrackingItems.Add(trackingItem);
            }

            return requestModel;
        }
    }
}
