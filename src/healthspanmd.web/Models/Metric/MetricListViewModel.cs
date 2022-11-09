using System.Collections.Generic;

namespace healthspanmd.web.Models.Metric
{
    public class MetricListViewModel
    {

        public ICollection<MetricItem> Items { get; set; }

        public class MetricItem
        {
            public int MetricId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string MetricType { get; set; }
            public int UsersCount { get; set; }
            public bool IsActive { get; set; }

        }
    }
}
