using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class GetContentCardListQueryFilter
    {
        public bool? ActiveOnly { get; set; }
        public int? ContentTagId { get; set; }
    }
}
