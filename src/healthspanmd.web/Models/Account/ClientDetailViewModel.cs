using healthspanmd.core.CQRS.Metrics;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Account
{
    public class ClientDetailViewModel
    {
        public UserViewModel Client { get; set; }
        public ICollection<MetricModel> Metrics { get; set; }
        public string MetricsJson { get; set; }
        public string SelectedMetricsJson { get; set; }
    }
}
