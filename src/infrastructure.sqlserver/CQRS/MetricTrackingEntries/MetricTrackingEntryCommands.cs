using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.Services.ExceptionReporting;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.MetricTrackingEntries
{
    public class MetricTrackingEntryCommands : IMetricTrackingEntryCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public MetricTrackingEntryCommands(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }
        public AddMetricTrackingEntryResponse CreateOrReplaceEntries(AddMetricTrackingEntryRequest request)
        {
            var response = new AddMetricTrackingEntryResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();


                    // check for existing entries for this user/day
                    var existingEntries = dbContext.MetricTrackingEntries
                        .Where(e => e.UserId == request.TrackingItems.FirstOrDefault().UserId 
                                && e.EntryForDate == request.TrackingItems.FirstOrDefault().EntryForDate)
                        .ToList();
                    foreach (var entry in existingEntries)
                    {
                        dbContext.MetricTrackingEntries.Remove(entry);
                        dbContext.SaveChanges();
                    }


                    foreach (var item in request.TrackingItems)
                        dbContext.MetricTrackingEntries.Add(item.ToMetricTrackingEntry());

                    dbContext.SaveChanges();

                    response.Success = true;
                    response.MetricTrackingEntriesAddedCount = request.TrackingItems.Count;

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
