using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Domain
{
    public class UserDomainModel : IUserDomainModel
    {
        private readonly IUserQueries _userQueries;
        private readonly IMetricTrackingEntryCommands _metricTrackingEntryCommands;
        private readonly IUserCommands _userCommands;

        private UserModel _userDetailModel;

        public UserDomainModel(
            IUserQueries userQueries,
            IMetricTrackingEntryCommands metricTrackingEntryCommands,
            IUserCommands userCommands
            )
        {
            _userQueries = userQueries;
            _metricTrackingEntryCommands = metricTrackingEntryCommands;
            _userCommands = userCommands;
        }

        public void Init(string email)
        {
            _userDetailModel = _userQueries.GetUserDetailModel(email, true);
        }

        public void Init(long userId)
        {
            _userDetailModel = _userQueries.GetUserDetailModel(userId, true);
        }

        public UserModel UserDetail => _userDetailModel;
        public IUserCommands UserCommands => _userCommands;

        public AddMetricTrackingEntryResponse CreateMetricTrackingEntries(AddMetricTrackingEntryRequest request)
        {
            return _metricTrackingEntryCommands.CreateOrReplaceEntries(request);
        }

    }
}
