using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Interfaces
{
    public interface IQuestionarioGrupoRepository : IRepository<QuestionarioGrupo>
    {
        IEnumerable<QuestionarioGrupo> GetAtivosByGrupo(int idGrupo);

        QuestionarioGrupo GetByQuestionarioAndGrupo(int idQuestionario, int idGrupo);

        IEnumerable<QuestionarioGrupo> GetByQuestionario(int idQuestionario);
    }
}
