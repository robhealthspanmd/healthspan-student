using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentCard_Mapping
    {
        public static ContentCardModel ToContentCardModel(this ContentCard c)
        {
            return new ContentCardModel
            {
                ContentCardId = c.ContentCardId,
                ContentCardMembers = c.ContentCardMembers != null ? c.ContentCardMembers.Select(m => m.ToContentCardMemberModel()).ToList() : null,
                Name = c.Name,
                IsActive = c.IsActive,
                Description = c.Description,
                ImageFileId = c.ImageFileId,
                NotificationMessage = c.NotificationMessage,
                ContentTagAssignments = c.ContentTagAssignments != null ? c.ContentTagAssignments.Select(a => a.ToContentTagAssignmentModel()).ToList() : null, 
            };
        }

        public static ContentCard ToContentCard(this ContentCardModel c)
        {
            return new ContentCard
            {
                ContentCardId = c.ContentCardId,
                ContentCardMembers = c.ContentCardMembers != null ? c.ContentCardMembers.Select(m => m.ToContentCardMember()).ToList() : null,
                Name = c.Name,
                IsActive = c.IsActive,
                Description = c.Description,
                ImageFileId = c.ImageFileId,
                NotificationMessage = c.NotificationMessage,
                ContentTagAssignments = c.ContentTagAssignments != null ? c.ContentTagAssignments.Select(a => a.ToContentTagAssignment()).ToList() : null,
            };
        }
    }
}
