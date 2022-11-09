using healthspanmd.core.CQRS.MetricTypes;
using healthspanmd.core.Services.ExceptionReporting;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.MetricTypes
{
    public class MetricTypeQueries : IMetricTypeQueries
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public MetricTypeQueries(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }

        public MetricTypeModel GetMetricType(int metricTypeId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var metricType = dbContext.MetricTypes.Find(metricTypeId);

                return metricType.ToMetricTypeModel();
               
            }
        }

        public ICollection<MetricTypeModel> GetMetricTypes(GetMetricTypesQueryFilter filter)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var query = dbContext.MetricTypes.AsQueryable();

                ApplyFilter(query, filter);

                return query.Select(mt => mt.ToMetricTypeModel()).ToList();
            }
        }

        private void ApplyFilter(IQueryable<MetricType> query, GetMetricTypesQueryFilter filter)
        {

        }

    }
}
