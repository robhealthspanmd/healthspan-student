using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentTagAssignment_Mapping
    {
        public static ContentTagAssignmentModel ToContentTagAssignmentModel(this ContentTagAssignment a)
        {
            return new ContentTagAssignmentModel
            {
                ContentTagAssignmentId = a.ContentTagAssignmentId,
                ContentTagId = a.ContentTagId,
                ContentCardId = a.ContentCardId,
                ContentTag = a.ContentTag != null ? a.ContentTag.ToContentTagModel() : null
            };
        }

        public static ContentTagAssignment ToContentTagAssignment(this ContentTagAssignmentModel a)
        {
            return new ContentTagAssignment
            {
                ContentTagAssignmentId = a.ContentTagAssignmentId,
                ContentTagId = a.ContentTagId,
                ContentCardId = a.ContentCardId,
                ContentTag = a.ContentTag != null ? a.ContentTag.ToContentTag() : null
            };
        }
    }
}
