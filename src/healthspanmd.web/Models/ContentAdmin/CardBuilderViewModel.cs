using healthspanmd.core.CQRS.Content;
using System.Collections.Generic;
using System.Linq;

namespace healthspanmd.web.Models.ContentAdmin
{
    public class CardBuilderViewModel
    {
        public ContentCardModel ContentCard { get; set; }

        public string CardImageData { get; set; }

        public ICollection<ContentTagModel> ContentTags { get; set; }
        public string ContentTagsAsCommaDelimitedAndQuoted
        {
            get
            {
                var tagNames = ContentTags.Select(t => "\"" + t.Name + "\"").ToList();
                return string.Join(",", tagNames);
            }
        }
    }
}
