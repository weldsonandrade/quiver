using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Repository
{
    public class AvaliacaoQuestionarioGrupoRepository : GenericRepository<AvaliacaoQuestionarioGrupo>, IAvaliacaoQuestionarioGrupoRepository
    {
        public AvaliacaoQuestionarioGrupoRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<AvaliacaoQuestionarioGrupo> GetNaoAvaliadosByGrupo(int idGrupo)
        {
            return Get(aqg => aqg.IdGrupo == idGrupo && aqg.Avaliacao.Situacao == SituacaoAvaliacao.NAO_AVALIADO);
        }

        public IEnumerable<AvaliacaoQuestionarioGrupo> GetNaoAvaliadosByQuestionario(int idQuestionario)
        {
            return Get(aqg => aqg.IdQuestionario == idQuestionario && aqg.Avaliacao.Situacao == SituacaoAvaliacao.NAO_AVALIADO);
        }

        public IEnumerable<AvaliacaoQuestionarioGrupo> GetNaoAvaliadosByQuestionarioAndGrupo(int idQuestionarioGrupo, int idGrupo)
        {
            return Get(aqg => aqg.IdQuestionario == idQuestionarioGrupo && aqg.IdGrupo == idGrupo && aqg.Avaliacao.Situacao == SituacaoAvaliacao.NAO_AVALIADO);
        }
    }
}
