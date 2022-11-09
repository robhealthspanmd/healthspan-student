using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentItemModel
    {
        public int ContentItemId { get; set; }

        public string Name { get; set; }

        public ContentItemType ItemType { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }
        public ContentFileModel ContentFile { get; set; }
        public int? ContentFileId { get; set; }
        public bool IsCaption { get; set; }
    }
}
