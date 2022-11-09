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
    public class MetricCommands : IMetricCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public MetricCommands(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }

        

        public UpdateMetricResponse CreateOrUpdate(MetricModel model)
        {
            var response = new UpdateMetricResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    Metric metric = null;
                    if (model.MetricId < 1)
                    {
                        // Create new metric
                        metric = model.ToMetric();
                        dbContext.Metrics.Add(metric);
                    }
                    else
                    {
                        // update existing metric
                        metric = dbContext.Metrics.AsQueryable()
                            .Where(m => m.MetricId == model.MetricId)
                            .Include(m => m.SelectItems)
                            .FirstOrDefault();
                        metric.Name = model.Name;
                        metric.Description = model.Description;
                        metric.DataType = model.DataType;
                        metric.MinValue = model.MinValue;
                        metric.MaxValue = model.MaxValue;
                        metric.AllowMultipleValues = model.AllowMultipleValues;
                        metric.IsActive = model.IsActive;
                        metric.SelectItems = model.SelectItems != null ? model.SelectItems.Select(s => s.ToMetricSelectItem()).ToList() : null;
                        metric.Frequency = model.Frequency;
                        metric.Threshold = model.Threshold;
                        metric.Threshold2 = model.Threshold2;
                        metric.IsAlphaSelectNumeric = model.IsAlphaSelectNumeric;
                        metric.IsPositivePolarity = model.IsPositivePolarity;
                        dbContext.Metrics.Update(metric);
                    }

                    dbContext.SaveChanges();

                    model = metric.ToMetricModel();

                    response.Success = true;
                    response.Metric = model;
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

        public UpdateMetricResponse Delete(int metricId)
        {
            var response = new UpdateMetricResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var metric = dbContext.Metrics.Find(metricId);
                    dbContext.Metrics.Remove(metric);
                    dbContext.SaveChanges();

                    response.Success = true;
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

        public UpdateMetricResponse SetActiveStatus(int metricId, bool isActive)
        {
            var response = new UpdateMetricResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var metric = dbContext.Metrics.Find(metricId);
                    metric.IsActive = isActive;

                    dbContext.Metrics.Update(metric);
                    dbContext.SaveChanges();

                    response.Success = true;
                    response.Metric = metric.ToMetricModel();
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
