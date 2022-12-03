using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentTag_Mapping
    {
        public static ContentTagModel ToContentTagModel(this ContentTag t)
        {
            return new ContentTagModel
            {
                ContentTagId = t.ContentTagId,
                Description = t.Description,
                Name = t.Name,
            };
        }

        public static ContentTag ToContentTag(this ContentTagModel t)
        {
            return new ContentTag
            {
                ContentTagId = t.ContentTagId,
                Description = t.Description,
                Name = t.Name,
            };
        }
    }
}
