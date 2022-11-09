using healthspanmd.core.CQRS.Users;
using healthspanmd.web.Models.Content;
using System.Collections;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Home
{
    public class DashboardViewModel
    {

        public UserModel User { get; set; } 

        public ICollection<ContentCardPreviewViewModel> ContentCardPreviews { get; set; }

       

    }
}
