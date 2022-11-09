using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Domain
{
    public class DomainFactory
    {

        private readonly IServiceProvider _serviceProvider;

        public DomainFactory(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IUserDomainModel> UserAsync(string email)
        {
            var u = (IUserDomainModel)_serviceProvider.GetService(typeof(IUserDomainModel));
            u.Init(email);
            return u;
        }

        public async Task<IUserDomainModel> UserAsync(long userId)
        {
            var u = (IUserDomainModel)_serviceProvider.GetService(typeof(IUserDomainModel));
            u.Init(userId);
            return u;
        }
    }
}
