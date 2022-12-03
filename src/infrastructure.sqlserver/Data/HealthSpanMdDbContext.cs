using infrastructure.sqlserver.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data
{
    public class HealthSpanMdDbContext : DbContext
    {
        public HealthSpanMdDbContext(DbContextOptions<HealthSpanMdDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricSelectItem> MetricsSelectItems { get; set; }
        //public DbSet<MetricType> MetricTypes { get; set; }
        public DbSet<ActiveMetric> ActiveMetrics { get; set; }
        public DbSet<MetricTrackingEntry> MetricTrackingEntries { get; set; }
        public DbSet<MetricTrackingEntryValue> MetricTrackingEntriesValues { get; set;}
        public DbSet<AuthorizedRole> AuthorizedRoles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<ContentCard> ContentCards { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<ContentFile> ContentFiles { get; set; }
        public DbSet<ContentCardAssignment> ContentCardsAssignments { get; set; }
        public DbSet<ContentCardMember> ContentCardsMembers { get; set; }
        public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<ContentTagAssignment> ContentTagAssignments { get; set; }
    }
}
