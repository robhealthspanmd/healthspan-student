﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ContentCardMember
    {
        public int ContentCardMemberId { get; set; }
        public int ContentCardId { get; set; }
        public int ContentItemId { get; set; }
        public ContentItem ContentItem { get; set; }
        public int SortOrder { get; set; }
    }
}
