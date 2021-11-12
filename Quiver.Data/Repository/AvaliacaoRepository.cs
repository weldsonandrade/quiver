using System;
using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Quiver.Common.Utils;
using System.Diagnostics;

namespace Quiver.Data.Repository
{
    public class AvaliacaoRepository : GenericRepository<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<Avaliacao> GetByUnidadeAndSituacao(int unidadeId, SituacaoAvaliacao situacao)
        {
            return Get(a => a.IdUnidade == unidadeId && a.Situacao == SituacaoAvaliacao.NAO_AVALIADO);
        }

        public IEnumerable<Avaliacao> GetByGrupoAndSituacao(int grupoId, SituacaoAvaliacao situacao)
        {
            return Get(a => a.Situacao == SituacaoAvaliacao.NAO_AVALIADO && a.QuestionariosAvaliacao.FirstOrDefault().QuestionarioGrupo.IdGrupo == grupoId);
        }

        public Avaliacao GetById(int avaliacaoId)
        {
            return Get(a => a.Id == avaliacaoId).FirstOrDefault();
        }

        public Avaliacao GetAvaliadaById(int avaliacaoId)
        {
            Avaliacao avaliacao = Get(a => a.Id == avaliacaoId && a.Situacao == SituacaoAvaliacao.AVALIADO,
                includeProperties: "QuestionariosAvaliacao.Respostas").FirstOrDefault();

            if (avaliacao != null)
            {
                foreach(AvaliacaoQuestionarioGrupo aq in avaliacao.QuestionariosAvaliacao)
                {
                    aq.QuestionarioGrupo.Questionario.Questoes = aq.QuestionarioGrupo.Questionario.Questoes.OrderBy(q => q.Ordem).ToList();
                }
            }

            return avaliacao;
        }

        public IEnumerable<Avaliacao> GetByEmpresaAndPeriodo(int idEmpresa, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(x => x.DataProgramada >= dataInicial && x.DataProgramada <= dataFinal && x.Unidade.Empresa.Id == idEmpresa, includeProperties: "Unidade");
        }

        public IEnumerable<Avaliacao> GetByEmpresaAndStartWithRotulo(int idEmpresa, string termo)
        {
            return Get(filter: g => g.RotuloCalendario.StartsWith(termo) && g.Unidade.Empresa.Id == idEmpresa);
        }

        public IEnumerable<Avaliacao> GetByUsuario(string idUsuario)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: a => a.IdUsuario == idUsuario).OrderBy(a => a.DataProgramada);
        }

        public IEnumerable<Avaliacao> GetByUnidade(int idUnidade)
        {
            return Get(filter: a => a.IdUnidade == idUnidade).OrderBy(a => a.DataProgramada);
        }

        public IEnumerable<Avaliacao> GetByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: a => a.IdUsuario == idUsuario
                                    && a.DataProgramada >= dataInicial
                                    && a.DataProgramada <= dataFinal,
                            includeProperties: "Unidade,QuestionariosAvaliacao,QuestionariosAvaliacao.Questionario,QuestionariosAvaliacao.Questionario.Grupo")
                       .OrderByDescending(a => a.DataProgramada);
        }

        public IEnumerable<Avaliacao> GetFinalizadasByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: a => a.IdUsuario == idUsuario
                        && a.Situacao == SituacaoAvaliacao.AVALIADO
                        && a.DataInicio >= dataInicial
                        && a.DataFim <= dataFinal,
                        includeProperties: "Unidade")
                       .OrderBy(a => a.DataFim);
        }


        public IEnumerable<Avaliacao> GetFinalizadasComPontosByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: a => a.PontuacaoMaxima > 0 
                                    && a.IdUsuario == idUsuario 
                                    && a.Situacao == SituacaoAvaliacao.AVALIADO 
                                    && a.DataInicio >= dataInicial 
                                    && a.DataFim <= dataFinal,
                            includeProperties: "Unidade")
                       .OrderBy(a => a.DataFim);
        }


        public IEnumerable<Avaliacao> GetByEmpresa(int idEmpresa)
        {
            return Get(filter: a => a.Usuario.Empresa.Id == idEmpresa);
        }

        public IEnumerable<Avaliacao> GetAtrasadasByEmpresa(int idEmpresa)
        {
            DateTime dataAtual = TZUtil.GetDataDeBrasilia();
            return Get(a => a.Situacao == SituacaoAvaliacao.NAO_AVALIADO && a.DataProgramada < dataAtual && a.Usuario.Empresa.Id == idEmpresa);
        }

        public IEnumerable<Avaliacao> GetEmAndamentoByEmpresa(int idEmpresa)
        {
            DateTime dataAtual = TZUtil.GetDataDeBrasilia();
            return Get(a => a.Situacao == SituacaoAvaliacao.NAO_AVALIADO && a.DataProgramada >= dataAtual && a.Usuario.Empresa.Id == idEmpresa);
        }

        public IEnumerable<Avaliacao> GetFinalizadasByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(a => a.IdUnidade == idUnidade && a.DataFim >= dataInicial && a.DataFim <= dataFinal && a.Situacao == SituacaoAvaliacao.AVALIADO, includeProperties: "Unidade");
        }

        public IEnumerable<Avaliacao> GetFinalizadasComPontosByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(a => a.PontuacaoMaxima > 0 && a.IdUnidade == idUnidade && a.DataFim >= dataInicial && a.DataFim <= dataFinal && a.Situacao == SituacaoAvaliacao.AVALIADO, includeProperties: "Unidade");
        }

        public IEnumerable<Avaliacao> GetFinalizadasByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: a => a.QuestionariosAvaliacao.FirstOrDefault().QuestionarioGrupo.IdGrupo == idGrupo
                            && a.DataInicio >= dataInicial
                            && a.DataFim <= dataFinal
                            && a.Situacao == SituacaoAvaliacao.AVALIADO,
                            includeProperties: "Unidade");
        }

        public IEnumerable<Avaliacao> GetFinalizadasComPontosByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: a => a.QuestionariosAvaliacao.FirstOrDefault().QuestionarioGrupo.IdGrupo == idGrupo
                            && a.DataInicio >= dataInicial
                            && a.DataFim <= dataFinal
                            && a.Situacao == SituacaoAvaliacao.AVALIADO
                            && a.PontuacaoMaxima > 0,
                            includeProperties: "Unidade");
        }

        public IEnumerable<Avaliacao> GetByEmpresaAndPeriodoAndUnidadeAndUsuario(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, string idUsuario)
        {
            context.Configuration.LazyLoadingEnabled = false;

            try
            {
                var avaliacoes = (from avaliacao in dbSet
                             where avaliacao.Unidade.IdEmpresa == idEmpresa
                             //&& avaliacao.DataProgramada > dataInicio && avaliacao.DataProgramada <= DateTime.Now
                             && (idUnidade == 0 || idUnidade == avaliacao.IdUnidade)
                             && (string.IsNullOrEmpty(idUsuario) || idUsuario == avaliacao.IdUsuario)
                             select new { Id = avaliacao.Id, Agendada = avaliacao.Agendada, Conforme = avaliacao.Conforme,
                                          Titulo = avaliacao.RotuloCalendario, DataProgramada = avaliacao.DataProgramada,
                                          Situacao = avaliacao.Situacao})
                    .ToList()
                    .Where(a => a.DataProgramada >= dataInicial && a.DataProgramada <= dataFinal)
                             .Select(x => new Avaliacao { Id = x.Id, Agendada = x.Agendada, Conforme = x.Conforme, RotuloCalendario = x.Titulo,
                                                         DataProgramada = x.DataProgramada, Situacao = x.Situacao});

                List<Avaliacao> avaliacoesModel = new List<Avaliacao>();

                //foreach (var avaliacao in avaliacoes)
                //{
                //    Avaliacao newAvaliacao = new Avaliacao();

                //    newAvaliacao.Id = avaliacao.Id;
                //    newAvaliacao.Agendada = avaliacao.Agendada;
                //    newAvaliacao.Conforme = avaliacao.Conforme;
                //    newAvaliacao.RotuloCalendario = avaliacao.Titulo;
                //    newAvaliacao.Situacao = avaliacao.Situacao;
                //    newAvaliacao.DataProgramada = avaliacao.DataProgramada;

                //    avaliacoesModel.Add(newAvaliacao);
                //}

                return avaliacoes;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<Avaliacao> GetNaoAvaliadasByQuestionario(int idQuestionario)
        {
            return Get(a => a.Situacao == SituacaoAvaliacao.NAO_AVALIADO && a.QuestionariosAvaliacao.Where(q => q.IdQuestionario == idQuestionario).Count() > 0);
        }

        public int GetIdEmpresaById(int idAvaliacao)
        {
            return (from avaliacao in dbSet where avaliacao.Id == idAvaliacao select avaliacao.Usuario.IdEmpresa).FirstOrDefault();
        }

        public IEnumerable<Avaliacao> GetByFilter(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, string idUsuario, 
            int idGrupo, bool? apenasAgendadas, bool? apenasConformes)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(a => a.Unidade.IdEmpresa == idEmpresa
                && a.DataProgramada >= dataInicial
                && a.DataProgramada <= dataFinal
                && (idUnidade != 0 ? a.IdUnidade == idUnidade : 1 == 1)
                && (idUsuario != "0" ? a.IdUsuario == idUsuario : 1 == 1)
                && (idGrupo != 0 ? a.QuestionariosAvaliacao.Any(aqg => aqg.IdGrupo == idGrupo) : 1 == 1)
                && (apenasAgendadas != null ? a.Agendada == apenasAgendadas : 1 == 1)
                && (apenasConformes != null ? a.Conforme == apenasConformes : 1 == 1),
                includeProperties: "Unidade",
                orderBy: o => o.OrderBy(v => v.DataProgramada));
        }

        public IEnumerable<Avaliacao> GetAvaliadasByQuestionario(int idQuestionario)
        {
            return Get(a => a.Situacao == SituacaoAvaliacao.AVALIADO && a.QuestionariosAvaliacao.Any(aqg => aqg.IdQuestionario == idQuestionario));
        }
    }
}
