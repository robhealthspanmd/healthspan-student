using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentCardMember_Mapping
    {
        public static ContentCardMemberModel ToContentCardMemberModel(this ContentCardMember m)
        {
            return new ContentCardMemberModel
            {
                ContentCardMemberId = m.ContentCardMemberId,
                ContentCardId = m.ContentCardId,
                ContentItem = m.ContentItem.ToContentItemModel(),
                ContentItemId = m.ContentItemId,
                SortOrder = m.SortOrder
            };
        }

        public static ContentCardMember ToContentCardMember(this ContentCardMemberModel m)
        {
            return new ContentCardMember
            {
                ContentCardMemberId = m.ContentCardMemberId,
                ContentCardId = m.ContentCardId,
                ContentItem = m.ContentItem.ToContentItem(),
                ContentItemId = m.ContentItemId,
                SortOrder = m.SortOrder
            };
        }
    }
}
