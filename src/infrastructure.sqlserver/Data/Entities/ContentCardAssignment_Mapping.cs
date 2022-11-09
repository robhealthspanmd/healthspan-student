using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentCardAssignment_Mapping
    {
        public static ContentCardAssignmentModel ToContentCardAssignmentModel(this ContentCardAssignment c)
        {
            return new ContentCardAssignmentModel
            {
                ContentCardAssignmentId = c.ContentCardAssignmentId,
                AssignedBy = c.AssignedBy,
                CompletedUtc = c.CompletedUtc,
                ContentCard = c.ContentCard != null ? c.ContentCard.ToContentCardModel() : null,
                ContentCardId = c.ContentCardId,
                CreatedUtc = c.CreatedUtc,
                SortOrder = c.SortOrder,
                UserId = c.UserId,
                NotificationMessage = c.NotificationMessage,
                NotificationId = c.NotificationId,
            };
        }

        public static ContentCardAssignment ToContentCardAssignment(this ContentCardAssignmentModel c)
        {
            return new ContentCardAssignment
            {
                ContentCardAssignmentId = c.ContentCardAssignmentId,
                AssignedBy = c.AssignedBy,
                CompletedUtc = c.CompletedUtc,
                ContentCard = c.ContentCard != null ? c.ContentCard.ToContentCard() : null,
                ContentCardId = c.ContentCardId,
                CreatedUtc = c.CreatedUtc,
                SortOrder = c.SortOrder,
                UserId = c.UserId,
                NotificationMessage= c.NotificationMessage,
                NotificationId= c.NotificationId,
            };
        }
    }
}
