using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentTagAssignmentModel
    {
        public int ContentTagAssignmentId { get; set; }
        public int ContentTagId { get; set; }
        public ContentTagModel ContentTag { get; set; }
        public int ContentCardId { get; set; }
    }
}
