using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Quiver.Data.Repository
{
    public class QuestionarioRepository : GenericRepository<Questionario>, IQuestionarioRepository
    {
        public QuestionarioRepository(QuiverDbContext context) : base(context) { }

        public Questionario GetAtivoById(int idQuestionario)
        {
            return Get(filter: q => q.Id == idQuestionario && q.Excluido == false).FirstOrDefault();
        }

        public IEnumerable<Questionario> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: q => q.Nome.StartsWith(startWithNome)
                && q.Excluido == false
                && q.IdEmpresa == idEmpresa,
                includeProperties: "Grupos,Grupos.Grupo",
                orderBy: o => o.OrderBy(v => v.Nome));
        }

        public IEnumerable<Questionario> GetAtivosByGrupo(int idGrupo)
        {
            return Get(q => q.Grupos.Any(qg => qg.Grupo.Id == idGrupo) && q.Excluido == false);
        }
    }
}
