using healthspanmd.core.CQRS.MetricTrackingEntries;
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

namespace infrastructure.sqlserver.CQRS.MetricTrackingEntries
{
    public class MetricTrackingEntryQueries : IMetricTrackingEntryQueries
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public MetricTrackingEntryQueries(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }

        public ICollection<MetricTrackingEntryModel> GetList(GetMetricTrackingEntriesQueryFilter filter)
        {
            
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var entries = dbContext.MetricTrackingEntries.AsQueryable();
                        

                    if (filter.UserId.HasValue)
                        entries = entries.Where(e => e.UserId == filter.UserId.Value);

                    if (filter.MetricId.HasValue)
                        entries = entries.Where(e => e.MetricId == filter.MetricId.Value);

                    if (filter.StartDate.HasValue)
                        entries = entries.Where(e => e.EntryForDate >= filter.StartDate.Value);

                    if (filter.EndDate.HasValue)
                        entries = entries.Where(e => e.EntryForDate <= filter.EndDate.Value);

                    entries = entries.Include(e => e.EntryValues);

                    return entries.Select(e => e.ToMetricTrackingEntryModel()).ToList();


                }
            }
            catch (Exception ex)
            {

                _exceptionReporter.Execute(ex, this.GetType());
                return null;
            }

        }
    }
}
