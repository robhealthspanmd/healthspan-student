using System.Collections.Generic;

namespace healthspanmd.web.Models.ContentAdmin
{
    public class ContentCardSortOrderModel
    {
        public int ContentCardId { get; set; }
        public List<int> ContentItemIds { get; set; }
    }
}
