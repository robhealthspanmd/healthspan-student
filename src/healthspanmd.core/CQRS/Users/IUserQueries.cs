using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Users
{
    public interface IUserQueries
    {

        UserModel GetUserDetailModel(string email, bool includeMetricTrackingEntries);
        UserModel GetUserDetailModel(long userId, bool includeMetricTrackingEntries);
        ICollection<UserModel> GetUserList(GetUserListQueryFilter filter);

    }
}
