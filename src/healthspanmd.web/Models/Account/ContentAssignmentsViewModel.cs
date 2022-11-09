using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Users;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Account
{
    public class ContentAssignmentsViewModel
    {
        public UserModel TargetUser { get; set; }
        public ICollection<ContentCardModel> ContentCardLibrary { get; set; }
    }
}
