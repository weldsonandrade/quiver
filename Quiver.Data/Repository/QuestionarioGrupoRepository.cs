using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Repository
{
    public class QuestionarioGrupoRepository : GenericRepository<QuestionarioGrupo>, IQuestionarioGrupoRepository
    {
        public QuestionarioGrupoRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<QuestionarioGrupo> GetAtivosByGrupo(int idGrupo)
        {
            return Get(q => q.Questionario.Excluido == false && q.IdGrupo == idGrupo);
        }

        public IEnumerable<QuestionarioGrupo> GetByQuestionario(int idQuestionario)
        {
            return Get(qg => qg.IdQuestionario == idQuestionario);
        }

        public QuestionarioGrupo GetByQuestionarioAndGrupo(int idQuestionario, int idGrupo)
        {
            return Get(q => q.IdQuestionario == idQuestionario && q.IdGrupo == idGrupo).FirstOrDefault();
        }
    }
}
