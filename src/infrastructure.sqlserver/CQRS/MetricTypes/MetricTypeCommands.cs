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
    public class MetricTypeCommands : IMetricTypeCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public MetricTypeCommands(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }
        public UpdateMetricTypeResponse Create(MetricTypeModel model)
        {
            var response = new UpdateMetricTypeResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var metricType = model.ToMetricType();

                    dbContext.MetricTypes.Add(metricType);
                    dbContext.SaveChanges();
                    model.MetricTypeId = metricType.MetricTypeId;

                    response.Success = true;
                    response.Message = $"MetricType {model.Name} [{model.MetricTypeId}] was created.";
                    response.MetricType = model;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }
            return response;
        }
    }
}
