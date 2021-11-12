using Quiver.Core.Models;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IUnidadeRepository : IRepository<Unidade>
    {
        IEnumerable<Unidade> GetAtivosByEmpresa(int IdEmpresa);

        IEnumerable<Unidade> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome);

        int GetIdEmpresaByIdUnidade(int idUnidade);
    }
}
