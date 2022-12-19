using healthspanmd.core.CQRS.Content;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Content
{
    public class ContentTagListingViewModel
    {
        public ICollection<ContentCardModel> TaggedContent { get; set; }
        public ICollection<ContentTagModel> Tags { get; set; }
        public ContentTagModel SelectedTag { get; set; }
    }
}
