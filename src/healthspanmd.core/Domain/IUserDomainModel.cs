using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Domain
{
    public interface IUserDomainModel
    {
        void Init(string email);
        void Init(long userId);
        UserModel UserDetail { get; }
        IUserCommands UserCommands { get; }
        AddMetricTrackingEntryResponse CreateMetricTrackingEntries(AddMetricTrackingEntryRequest request);
    }
}
