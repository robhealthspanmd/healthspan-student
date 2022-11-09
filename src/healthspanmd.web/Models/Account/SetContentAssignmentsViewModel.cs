using System.Collections.Generic;

namespace healthspanmd.web.Models.Account
{
    public class SetContentAssignmentsViewModel
    {
        public long UserId { get; set; }
        public List<int> ContentCardIds { get; set; }
    }
}
