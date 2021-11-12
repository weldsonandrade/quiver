using System;
using System.Collections.Generic;
using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Repository
{
    public class PlanoAcaoRepository : GenericRepository<PlanoAcao>, IPlanoAcaoRepository
    {
        public PlanoAcaoRepository(QuiverDbContext context) : base(context) { }

        new public virtual void Update(PlanoAcao planoAcaoToUpdate)
        {
            Update(planoAcaoToUpdate, true);
        }

        public virtual void Update(PlanoAcao planoAcaoToUpdate, bool autoHistorico)
        {
            if (autoHistorico)
            {
                var changeInfo = context.ChangeTracker.Entries<PlanoAcao>()
                    .Where(t => t.State == EntityState.Modified)
                    .Select(t => new
                    {
                        Original = t.OriginalValues.PropertyNames.ToDictionary(pn => pn, pn => t.OriginalValues[pn]),
                        Current = t.CurrentValues.PropertyNames.ToDictionary(pn => pn, pn => t.CurrentValues[pn])
                    });

                foreach (var propriedade in changeInfo)
                {
                    StringBuilder oldValues = new StringBuilder();
                    foreach(var p in propriedade.Original)
                    {
                        oldValues.Append(p.Key + " = " + p.Value);
                        if (p.Key != propriedade.Original.Last().Key)
                        {
                            oldValues.Append(", ");
                        }
                    }

                    StringBuilder currentValues = new StringBuilder();
                    foreach (var p in propriedade.Current)
                    {
                        currentValues.Append(p.Key + " = " + p.Value);
                        if (p.Key != propriedade.Current.Last().Key)
                        {
                            currentValues.Append(", ");
                        }
                    }

                    Historico historico = new Historico
                    {
                        DataModificacao = DateTime.Now,
                        Descricao = planoAcaoToUpdate.Justificativa,
                        // Aqui informaria a tabela.
                        NomeCampo = NomeCampoPlanoAcao.Como,
                        //NomeCampo = (NomeCampoPlanoAcao)Enum.Parse(typeof(NomeCampoPlanoAcao), propriedade.Propriedade.Name), //NomeCampo = NomeCampoPlanoAcao.Situacao,
                        PlanoAcao = planoAcaoToUpdate,
                        // Aqui se foi uma alteração pelo SISTEMA ou pelo USUARIO.
                        Tipo = TipoHistorico.Automatico,
                        ValorAntigo = oldValues.ToString(),
                        ValorNovo = currentValues.ToString()
                        // IdUsuario (se auterado pelo usuario).
                    };
                    planoAcaoToUpdate.Historicos.Add(historico);
                }
            }

            base.Update(planoAcaoToUpdate);
        }

        public IEnumerable<PlanoAcao> GetByEmpresaAndPeriodo(int idEmpresa, Nullable<DateTime> dataInicial, Nullable<DateTime> dataFinal)
        {
            IEnumerable<PlanoAcao> planosAcaoPorEmpresa = GetByEmpresa(idEmpresa);

            if (dataInicial != null && dataFinal != null) {
                return planosAcaoPorEmpresa.Where(p => p.Quando >= dataInicial && p.Quando <= dataFinal);
            } else if (dataInicial != null)
            {
                return planosAcaoPorEmpresa.Where(p => p.Quando >= dataInicial);
            } else
            {
                return planosAcaoPorEmpresa.Where(p => p.Quando <= dataFinal);
            }
        }

        public IEnumerable<PlanoAcao> GetByEmpresaAndResponsavelAndUnidadesAndUsuarios(int idEmpresa, string emailResponsavel, List<int> unidades, List<string> usuarios)
        {
            IEnumerable<PlanoAcao> planos = GetByEmpresa(idEmpresa);

            if (!string.IsNullOrEmpty(emailResponsavel))
            {
                planos = planos.Where(p => p.Responsavel == emailResponsavel);
            }

            if (unidades != null && unidades.Count > 0)
            {
                planos = planos.Where(p => unidades.Contains(p.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Unidade.Id));
            }

            if (usuarios != null && usuarios.Count > 0)
            {
                planos = planos.Where(p => usuarios.Contains(p.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Usuario.Id));
            }

            return planos;
        }

        public IEnumerable<PlanoAcao> GetByEmpresa(int idEmpresa)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return Get(filter: p => p.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Unidade.IdEmpresa == idEmpresa,
                includeProperties: "RespostaItem.Item.Questao.Itens.Alternativa,RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.QuestionarioGrupo.Questionario" +
                ",RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.QuestionarioGrupo.Grupo" +
                ",RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Unidade" +
                ",RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Usuario");
        }

    }
}
