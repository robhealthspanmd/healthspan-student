﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentCardAssignmentModel
    {
        public int ContentCardAssignmentId { get; set; }
        public int ContentCardId { get; set; }
        public ContentCardModel ContentCard { get; set; }
        public long UserId { get; set; }
        public long AssignedBy { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? CompletedUtc { get; set; }
        public string NotificationMessage { get; set; }
        public long? NotificationId { get; set; }
    }
}
