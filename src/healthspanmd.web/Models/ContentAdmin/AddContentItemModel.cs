using healthspanmd.core.CQRS.Content;

namespace healthspanmd.web.Models.ContentAdmin
{
    public class AddContentItemModel
    {
        public int ContentCardId { get; set; }
        public ContentItemType ItemType { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public int? ContentFileId { get; set; }
        public bool IsCaption { get; set; }
    }
}
