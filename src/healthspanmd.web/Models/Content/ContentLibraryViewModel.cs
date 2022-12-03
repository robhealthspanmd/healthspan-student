using healthspanmd.core.CQRS.Content;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Content
{
    public class ContentLibraryViewModel
    {
        public ICollection<ContentCardModel> AssignedContent { get; set; }
        public ICollection<ContentTagModel> Tags { get; set; }
    }
}
