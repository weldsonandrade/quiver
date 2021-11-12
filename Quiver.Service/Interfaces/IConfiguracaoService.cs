using Quiver.DTO.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IConfiguracaoService
    {
        /// <summary>
        /// Obtém a última versão mobile.
        /// </summary>
        /// <returns>Número da última versão.</returns>
        int GetLastMobileVersion();
    }
}
