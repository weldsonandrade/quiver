using Quiver.Core.Models;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IGrupoRepository : IRepository<Grupo>
    {
        IEnumerable<Grupo> GetAtivosByEmpresa(int IdEmpresa);

        IEnumerable<Grupo> GetAtivosByEmpresaWithAtLeastOneQuestionario(int IdEmpresa);

        IEnumerable<Grupo> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome);
    }
}
