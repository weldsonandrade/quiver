using Quiver.Core.Models;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IQuestionarioRepository : IRepository<Questionario>
    {
        IEnumerable<Questionario> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome);

        Questionario GetAtivoById(int idQuestionario);

        IEnumerable<Questionario> GetAtivosByGrupo(int idGrupo);
    }
}
