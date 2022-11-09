using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Domain
{
    public interface IDomainFactory
    {
        Task<IUserDomainModel> UserAsync(string email);
        Task<IUserDomainModel> UserAsync(long userId);
    }
}
