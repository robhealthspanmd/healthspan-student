using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentCardModel
    {
        public int ContentCardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ImageFileId { get; set; }
        public ICollection<ContentCardMemberModel> ContentCardMembers { get; set; }
        public bool IsActive { get; set; }
        public string NotificationMessage { get; set; }


    }
}
