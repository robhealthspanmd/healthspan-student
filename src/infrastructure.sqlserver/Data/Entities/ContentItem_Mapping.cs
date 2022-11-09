using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentItem_Mapping
    {
        public static ContentItemModel ToContentItemModel(this ContentItem c)
        {
            return new ContentItemModel
            {
                ContentItemId = c.ContentItemId,
                ItemType = c.ItemType,
                Name = c.Name,
                Text = c.Text,
                Url = c.Url,
                ContentFileId = c.ContentFileId,
                IsCaption = c.IsCaption,
                ContentFile = c.ContentFile != null ? c.ContentFile.ToContentFileModel() : null
            };
        }

        public static ContentItem ToContentItem(this ContentItemModel c)
        {
            return new ContentItem
            {
                ContentItemId = c.ContentItemId,
                ItemType = c.ItemType,
                Name = c.Name,
                Text = c.Text,
                Url = c.Url,
                ContentFileId = c.ContentFileId,
                IsCaption= c.IsCaption,
                ContentFile = c.ContentFile != null ? c.ContentFile.ToContentFile() : null
            };
        }
    }
}
