using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ContentCard
    {
        public int ContentCardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ImageFileId { get; set; }
        public ICollection<ContentCardMember> ContentCardMembers { get; set; }
        public bool IsActive { get; set; }
        public string NotificationMessage { get; set; }
    }
}
