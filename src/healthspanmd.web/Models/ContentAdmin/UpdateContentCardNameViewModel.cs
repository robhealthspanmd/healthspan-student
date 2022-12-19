using System.Collections.Generic;

namespace healthspanmd.web.Models.ContentAdmin
{
    public class UpdateContentCardNameViewModel
    {
        public int ContentCardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NotificationMessage { get; set; }
        public string SelectedTags { get; set; }

        
    }
}
