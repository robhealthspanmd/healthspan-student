using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Users;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models.Analytics;
using healthspanmd.web.Models.Metric;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static healthspanmd.web.Models.Analytics.ProgressViewModel;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {

        private readonly IUserQueries _userQueries;
        private readonly IMetricTrackingEntryQueries _trackingEntryQueries;

        public AnalyticsController(
            IUserQueries userQueries,
            IMetricTrackingEntryQueries trackingEntryQueries
            )
        {
            _userQueries = userQueries;
            _trackingEntryQueries = trackingEntryQueries;
        }

        public IActionResult ApexChart()
        {
            return View();
        }



        public IActionResult Index()
        {
            this.AddBreadCrumb("Progress");
            var user = _userQueries.GetUserDetailModel(User.Identity.Name, true);
            var model = GetProgressViewModel(user, TimePeriodType.Week, DateTime.Today.AddDays(-6));
            return View(model);
        }



        [Route("/Analytics/ProgressTable_Read")]
        public IActionResult ProgressTable_Read([DataSourceRequest] DataSourceRequest request, int metricId, string startDate, string timePeriod, long userId)
        {
            var user = _userQueries.GetUserDetailModel(userId, true);
            var trackingStartDate = startDate.JavascriptDateStringToDate();
            int daysInPeriod = 7;
            switch (timePeriod)
            {
                case "Week":
                    daysInPeriod = 7;
                    break;

                case "Month":
                    daysInPeriod = 30;
                    break;

                case "Year":
                    daysInPeriod = 365;
                    break;

            }
            var list = user.MetricTrackingEntries
                .Where(e => e.MetricId == metricId && e.EntryForDate >= trackingStartDate && e.EntryForDate <= trackingStartDate.AddDays(daysInPeriod))
                .OrderBy(e => e.EntryForDate)
                .Select(e => new TableItemViewModel
                {
                    TrackingDate = e.EntryForDate,
                    Value1 = (int)e.EntryValues.FirstOrDefault().ValueAsNumber,
                    Value2 = (int)e.EntryValues.FirstOrDefault().Value2AsNumber,
                    Goal1 = (int)user.ActiveMetrics.Where(am => am.MetricId == e.MetricId).FirstOrDefault().Goal,
                    Goal2 = (int)user.ActiveMetrics.Where(am => am.MetricId == e.MetricId).FirstOrDefault().Goal2,
                    BiggerIsBetter = e.Metric.IsPositivePolarity
                }).ToList();

            for (int i = 0; i < daysInPeriod; i++)
            {
                var reportDate = trackingStartDate.AddDays(i);
                var dataForDate = list.Where(d => d.TrackingDate == reportDate).FirstOrDefault();
                if (dataForDate == null)
                {
                    list.Add(new TableItemViewModel
                    {
                        TrackingDate = reportDate,
                        Value1 = null,
                        Value2 = null,
                    });
                }
            }

            var result = list
                .OrderByDescending(r => r.TrackingDate)
                .AsEnumerable();

            return Json(result.ToDataSourceResult(request));
        }



        [Authorize(Policy = "CanViewUserProgress")]
        [Route("/Analytics/Progress/{userId}")]
        public IActionResult Progress(long userId)
        {
            this.AddBreadCrumb("Progress");
            var user = _userQueries.GetUserDetailModel(userId, true);
            var model = GetProgressViewModel(user, TimePeriodType.Week, DateTime.Today.AddDays(-6));
            return View("Index", model);
        }


        [Authorize(Policy = "CanViewUserProgress")]
        [Route("/Analytics/Progress/{userId}/{timePeriodStr}/{startDateStr}")]
        public IActionResult Progress(long userId, string timePeriodStr, string startDateStr)
        {
            this.AddBreadCrumb("Progress");
            TimePeriodType timePeriod = timePeriodStr switch
            {
                "Week" => TimePeriodType.Week,
                "Month" => TimePeriodType.Month,
                "Year" => TimePeriodType.Year
            };
            var startDateParts = startDateStr.Split('-');
            var startDate = new DateTime(Convert.ToInt32(startDateParts[0]), Convert.ToInt32(startDateParts[1]), Convert.ToInt32(startDateParts[2]));

            // protect boundaries
            if (startDate > DateTime.Today)
            {
                startDate = timePeriod switch
                {
                    TimePeriodType.Week => DateTime.Today.AddDays(-6),
                    TimePeriodType.Month => DateTime.Today.AddDays(-30),
                    TimePeriodType.Year => DateTime.Today.AddDays(-364)
                };
            }

            UserModel user = null;
            if (userId == 0)
                user = _userQueries.GetUserDetailModel(User.Identity.Name, true);
            else
                user = _userQueries.GetUserDetailModel(userId, true);

            var model = GetProgressViewModel(user, timePeriod, startDate);


            return View("Index", model);
        }


        [Authorize(Policy = "CanViewUserProgress")]
        [Route("/Analytics/ProgressChart/{metricId}/{timePeriodStr}/{startDateStr}")]
        public IActionResult ProgressChart(int metricId, string timePeriodStr, string startDateStr)
        {
            TimePeriodType timePeriod = timePeriodStr switch
            {
                "Week" => TimePeriodType.Week,
                "Month" => TimePeriodType.Month,
                "Year" => TimePeriodType.Year
            };
            var startDateParts = startDateStr.Split('-');
            var startDate = new DateTime(Convert.ToInt32(startDateParts[0]), Convert.ToInt32(startDateParts[1]), Convert.ToInt32(startDateParts[2]));
            var user = _userQueries.GetUserDetailModel(User.Identity.Name, true);
            var model = GetProgressViewModel(user, timePeriod, startDate);

            return Json(new { success = true, options = model.TrackingDataSets.Where(tds => tds.MetricId == metricId).FirstOrDefault().OptionsJson });
        }

        private double MilliTimeStamp(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = TheDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return ts.TotalMilliseconds;
        }

        private ProgressViewModel GetProgressViewModel(UserModel user, TimePeriodType timePeriod, DateTime startDate)
        {
            var endDate = startDate;
            switch (timePeriod)
            {
                case TimePeriodType.Week:
                    endDate = startDate.AddDays(6);
                    break;

                case TimePeriodType.Month:
                    endDate = startDate.AddDays(30);
                    break;

                case TimePeriodType.Year:
                    endDate = startDate.AddDays(364);
                    break;
            }

            var pvm = new ProgressViewModel
            {
                TrackingDataSets = new List<MetricTrackingJson>(),
                TimePeriod = timePeriod,
                StartDate = startDate,
                ProgramStartDate = user.ProgramStartDate,
                User = user
            };

            foreach (var activeMetric in user.ActiveMetrics)
            {
                DateTime trackingDate = startDate;
                switch (activeMetric.Metric.DataType)
                {
                    case core.CQRS.Metrics.MetricDataType.Alpha_Select:
                    case core.CQRS.Metrics.MetricDataType.Numeric_Decimal:
                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer:
                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer_Dial:
                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer_Slider:
                    case core.CQRS.Metrics.MetricDataType.YesNo:

                        // if alpha select, but not numeric, then we ignore
                        if (activeMetric.Metric.DataType == core.CQRS.Metrics.MetricDataType.Alpha_Select)
                            if (!activeMetric.Metric.IsAlphaSelectNumeric)
                                break;

                        var dataset = new MetricTrackingJson
                        {
                            MetricId = activeMetric.MetricId,
                            MetricName = activeMetric.Metric.Name,
                            ChartType = "ChartOptions"
                        };

                        var series = new List<ChartOptions_SeriesData>();
                        var categories = new List<string>();
                        double maxTrackingDataValue = 0.0;

                        do
                        {
                            var trackingData = user.MetricTrackingEntries
                                .Where(e => e.MetricId == activeMetric.MetricId && e.EntryForDate == trackingDate)
                                .FirstOrDefault();
                            double? trackingDataValue = null;
                            if (trackingData != null)
                            {

                                // write the numeric value
                                switch (activeMetric.Metric.DataType)
                                {
                                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer:
                                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer_Dial:
                                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer_Slider:
                                        trackingDataValue = Convert.ToInt32(trackingData.EntryValues.FirstOrDefault().ValueAsNumber);
                                        break;

                                    case core.CQRS.Metrics.MetricDataType.Alpha_Select:
                                        if (double.TryParse(trackingData.EntryValues.FirstOrDefault().ValueAsString, out double convertedValue))
                                            trackingDataValue = convertedValue;
                                        break;

                                    case core.CQRS.Metrics.MetricDataType.YesNo:
                                        var boolValue = trackingData.EntryValues.FirstOrDefault();
                                        if (boolValue != null)
                                            trackingDataValue = boolValue.ValueAsBoolean.Value ? 1 : 0;
                                        break;

                                    default:
                                        trackingDataValue = trackingData.EntryValues.FirstOrDefault().ValueAsNumber;
                                        break;
                                }
                            }

                            if (trackingDataValue.HasValue)
                                if (trackingDataValue.Value > maxTrackingDataValue)
                                    maxTrackingDataValue = trackingDataValue.Value;

                            series.Add(new ChartOptions_SeriesData
                            {
                                X = MilliTimeStamp(trackingDate),
                                Y = trackingDataValue
                            });

                            //categories.Add(trackingDate.ToString("M-d"));
                            categories.Add(MilliTimeStamp(trackingDate).ToString());
                            trackingDate = trackingDate.AddDays(1);
                        } while (trackingDate <= endDate);

                        var options = new ChartOptions
                        {
                            Chart = new ChartOptions_Chart
                            {
                                ChartType = timePeriod == TimePeriodType.Year ? "scatter" : "area",
                                Toolbar = new ChartOptions_Chart_Toolbar
                                {
                                    Show = false
                                }
                            },
                            DataLabels = new ChartOptions_DataLabels
                            {
                                Enabled = true,
                                Formatter = activeMetric.Metric.DataType == core.CQRS.Metrics.MetricDataType.YesNo
                                                ? "function (val) { if (val === null) { return ''; } else { if (val == 1) { return 'Y';} else { return 'N'; }}}"
                                                : "function (val) { if (val === null) { return ''; } else { return val; }}"
                            },
                            Colors = new string[1] { "#00e396" },
                            Stroke = activeMetric.Metric.DataType == core.CQRS.Metrics.MetricDataType.YesNo ? new ChartOptions_Stroke { Curve = "straight" } : new ChartOptions_Stroke { Curve = "smooth" },
                            Markers = new ChartOptions_Markers { Size = 4 },
                            Series = new List<ChartOptions_Series> {
                                new ChartOptions_Series
                                {
                                    Name = activeMetric.Metric.Name,
                                    Data = series.ToArray(),
                                }
                            },
                            XAxis = new ChartOptions_XAxis { Type = "datetime" },
                        };

                        if (activeMetric.Goal.HasValue)
                        {
                            var bigValue = maxTrackingDataValue * 4;
                            if (bigValue == 0)
                                bigValue = activeMetric.Goal.Value * 20;
                            options.Annotations = new ChartOptions_Annotations
                            {
                                YAxis = new ChartOptions_Annotation_YAxis[]
                                {
                                    new ChartOptions_Annotation_YAxis
                                    {
                                        Y = activeMetric.Metric.IsPositivePolarity ? 0 : activeMetric.Goal.Value,
                                        Y2 = activeMetric.Metric.IsPositivePolarity ? activeMetric.Goal.Value : bigValue,
                                        BorderColor = "#000000",
                                        FillColor = "#ff0000",
                                        Opacity = 0.1,
                                        //Label = new ChartOptions_Label
                                        //{
                                        //    Text = "Goal",
                                        //    BorderColor = "#00ff00",
                                        //    Style = new ChartOptions_Style
                                        //    {
                                        //        Color = "#FFFFFF",
                                        //        Background = "#666666"
                                        //    }
                                        //}
                                        Label = new ChartOptions_Label
                                        {
                                            Text = "",
                                            Style = new ChartOptions_Style
                                            {

                                            }
                                        }
                                    }
                                }
                            };
                        }
                        else
                        {
                            options.Annotations = new ChartOptions_Annotations
                            {
                                YAxis = new ChartOptions_Annotation_YAxis[]
                                {
                                    new ChartOptions_Annotation_YAxis
                                    {
                                        Y = 0,
                                        Label = new ChartOptions_Label
                                        {
                                            Text = "",
                                            Style = new ChartOptions_Style
                                            {

                                            }
                                        }
                                    }
                                }
                            };
                        }




                        //dataset.OptionsJson = JsonSerializer.Serialize(options);
                        //var serializer = new Newtonsoft.Json.JsonSerializer();
                        dataset.OptionsJson = Newtonsoft.Json.JsonConvert.SerializeObject(options);
                        pvm.TrackingDataSets.Add(dataset);
                        break;

                    case core.CQRS.Metrics.MetricDataType.Numeric_Integer_2Values:
                        var dset = new MetricTrackingJson
                        {
                            MetricId = activeMetric.MetricId,
                            MetricName = activeMetric.Metric.Name,
                            ChartType = "Table"
                        };

                        
                        pvm.TrackingDataSets.Add(dset);
                        break;
                }
            }

            return pvm;
        }


    }
}
