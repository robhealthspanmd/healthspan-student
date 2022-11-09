using healthspanmd.core.CQRS.Metrics;
using healthspanmd.core.Services.ExceptionReporting;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.Metrics
{
    public class MetricQueries : IMetricQueries
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public MetricQueries(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }
        public MetricModel GetMetric(int metricId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var metric = dbContext.Metrics
                    .Include(m => m.SelectItems)
                    .Where(m => m.MetricId == metricId)
                    .SingleOrDefault();

                return metric.ToMetricModel();
            }
        }

        public ICollection<MetricModel> GetMetrics(GetMetricsQueryFilter filter, bool includeActiveUserAccount = false)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var query = dbContext.Metrics.AsQueryable()
                    .Include(m => m.SelectItems);
                ApplyFilter(query,filter);

                var result = query.Select(m => m.ToMetricModel()).ToList();

                if (includeActiveUserAccount)
                {
                    var activeMetrics = dbContext.ActiveMetrics.AsQueryable();
                    var userCountQueryDict = new Dictionary<int, int>();
                    foreach (var item in activeMetrics)
                    {
                        if (!userCountQueryDict.ContainsKey(item.MetricId))
                            userCountQueryDict.Add(item.MetricId, 0);
                        userCountQueryDict[item.MetricId] += 1;
                    }
                    foreach (var metric in result)
                        if (userCountQueryDict.ContainsKey(metric.MetricId))
                            metric.UserCountWithThisMetricAsActive = userCountQueryDict[metric.MetricId];
                }

                return result;
            }
        }

        private void ApplyFilter(IQueryable<Metric> query, GetMetricsQueryFilter filter)
        {

        }
    }
}
