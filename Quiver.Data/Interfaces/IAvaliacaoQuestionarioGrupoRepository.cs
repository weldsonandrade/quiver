using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Interfaces
{
    public interface IAvaliacaoQuestionarioGrupoRepository : IRepository<AvaliacaoQuestionarioGrupo>
    {
        IEnumerable<AvaliacaoQuestionarioGrupo> GetNaoAvaliadosByQuestionarioAndGrupo(int idQuestionario, int idGrupo);

        IEnumerable<AvaliacaoQuestionarioGrupo> GetNaoAvaliadosByQuestionario(int idQuestionario);

        IEnumerable<AvaliacaoQuestionarioGrupo> GetNaoAvaliadosByGrupo(int idGrupo);
    }
}
