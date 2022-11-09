using healthspanmd.core.CQRS.ActiveMetrics;
using healthspanmd.core.Services.ExceptionReporting;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.ActiveMetrics
{
    public class ActiveMetricCommands : IActiveMetricCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public ActiveMetricCommands(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }

        public void Create(ActiveMetricModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var activeMetric = model.ToActiveMetric();

                dbContext.ActiveMetrics.Add(activeMetric);
                dbContext.SaveChanges();

                model.ActiveMetricId = activeMetric.ActiveMetricId;

            }
        }
    }
}
