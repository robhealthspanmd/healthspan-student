using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentCardMemberModel
    {
        public int ContentCardMemberId { get; set; }
        public int ContentCardId { get; set; }
        public int ContentItemId { get; set; }
        public ContentItemModel ContentItem { get; set; }
        public int SortOrder { get; set; }
    }
}
