using System.Collections.Generic;

namespace healthspanmd.web.Models
{
    public class BreadCrumbViewModel
    {
        public ICollection<BreadCrumb> Items { get; set; }
        public class BreadCrumb
        {
            public string Caption { get; set; }
            public string Url { get; set; }
        }
    }
}
