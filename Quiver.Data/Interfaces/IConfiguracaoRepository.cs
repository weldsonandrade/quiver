using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Interfaces
{
    public interface IConfiguracaoRepository : IRepository<Configuracao>
    {
        /// <summary>
        /// Obtém configuração por nome.
        /// </summary>
        /// <returns>Configuração encontrada.</returns>
        Configuracao GetByName(String nome);

        /// <summary>
        /// Obtém a última versão da base de dados mobile.
        /// </summary>
        /// <returns>Valor da versão mais atual da base de dados.</returns>
        int GetLastMobileVersion();
    }
}
