using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ContentItem
    {
        public int ContentItemId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public ContentItemType ItemType { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public int? ContentFileId { get; set; }

        public ContentFile ContentFile { get; set; }

        public bool IsCaption { get; set; }

    }
}
