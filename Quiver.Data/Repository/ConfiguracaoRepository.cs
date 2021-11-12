using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Repository
{
    public class ConfiguracaoRepository : GenericRepository<Configuracao>, IConfiguracaoRepository
    {
        public ConfiguracaoRepository(QuiverDbContext context) : base(context) { }

        public Configuracao GetByName(string nome)
        {
            return Get(c => c.Nome == nome).FirstOrDefault();
        }

        public int GetLastMobileVersion()
        {
            return Convert.ToInt32(GetByName("LastMobileVersion").Valor);
        }
    }
}
