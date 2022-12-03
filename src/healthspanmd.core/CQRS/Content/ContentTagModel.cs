using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentTagModel
    {
        public int ContentTagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
