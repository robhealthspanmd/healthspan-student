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
        public ICollection<ContentTagAssignmentModel> ContentTagAssignments { get; set; }
        public bool ContainsAssignmentToTag(int tagId)
        {
            var assignment = ContentTagAssignments
                .Where(a => a.ContentTagId == tagId)
                .FirstOrDefault();
            return assignment != null;
        }
        public void SetTags(ICollection<int> tagIds)
        {
            // add new ones
            foreach (var tagId in tagIds)
                if (!this.ContainsAssignmentToTag(tagId))
                    ContentTagAssignments.Add(new ContentTagAssignmentModel
                    {
                        ContentCardId = this.ContentCardId,
                        ContentTagId = tagId
                    });

            // remove ones no longer there
            var tagIdsToRemove = new List<int>();
            foreach (var assignment in ContentTagAssignments)
                if (!tagIds.Contains(assignment.ContentTagId))
                    tagIdsToRemove.Add(assignment.ContentTagId);
            foreach (var tagId in tagIdsToRemove)
                ContentTagAssignments.Remove(ContentTagAssignments.Where(a => a.ContentTagId.Equals(tagId)).FirstOrDefault());
            
        }
        public string ContentCardTagIdsAsCommaDelimited
        {
            get
            {
                var ids = ContentTagAssignments.Select(a => a.ContentTag.ContentTagId).ToList();
                return string.Join(",", ids);
            }
        }

        public string ContentCardTagNamesAsCommaDelimited
        {
            get
            {
                var names = ContentTagAssignments.Select(a => a.ContentTag.Name).ToList();
                return string.Join(",", names);
            }
        }
    }
}
