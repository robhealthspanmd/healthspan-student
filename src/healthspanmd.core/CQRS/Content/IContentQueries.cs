using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public interface IContentQueries
    {
        ContentCardModel GetContentCard(int contentCardId);
        ContentFileModel GetContentFile(int contentFileId);
        ICollection<ContentCardModel> GetList(GetContentCardListQueryFilter filter);
        ICollection<ContentTagModel> GetAllContentTags();
        ICollection<ContentTagModel> GetContentTagsWithAssignments();

    }
}
