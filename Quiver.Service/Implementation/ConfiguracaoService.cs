using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Implementation
{
    public class ConfiguracaoService : IConfiguracaoService
    {
        private IUnitOfWork _uow;

        public ConfiguracaoService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public int GetLastMobileVersion()
        {
            return _uow.ConfiguracaoRepository.GetLastMobileVersion();
        }
    }
}
