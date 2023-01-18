using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ContentTagAssignment
    {
        public int ContentTagAssignmentId { get; set; }
        public int ContentTagId { get; set; }
        public ContentTag ContentTag { get; set; }
        public int ContentCardId { get; set; }
    }
}
