using Microsoft.AspNet.Identity;
using Quiver.Core.Models;
using Quiver.Data.Repository;
using Quiver.DTO.Models;
using Quiver.WebAPI.Mailers;
using Quiver.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Quiver.WebAPI.DTO
{
    public class PlanoAcaoDTO
    {
        private UnitOfWork _uow = new UnitOfWork();

        public void MarcarAtrasados()
        {
            //consulta os planos de acao que estao com data de conclusao anterior a data atual
            //em andamento e nao atrasado
            var planosAcao = 
                _uow.PlanoAcaoRepository.Get(p =>
                p.Situacao == SituacaoPlanoAcao.ANDAMENTO &&
                p.DataConclusao < DateTime.Today &&
                !p.Atrasado);

            foreach (PlanoAcao planoAcao in planosAcao)
            {
                Historico historico = GetModificacaoAutomatica(NomeCampoPlanoAcao.Situacao, planoAcao, DateTime.Now);
                // HistoricoPlanoAcao historico = new HistoricoPlanoAcao
                //{
                //    DataModificacao = DateTime.Now,
                //    Descricao = "Plano de ação marcado como atrasado pelo sistema.",
                //    NomeCampo = NomeCampoPlanoAcao.Atrasado,
                //    PlanoAcao = planoAcao,
                //    Tipo = TipoHistorico.Automatico,
                //    ValorAntigo = "Falso",
                //    ValorNovo = "Verdadeiro"
                //};

                planoAcao.Historicos.Add(historico);
                _uow.HistoricoRepository.Insert(historico);

                //marca os planos de acao como atrasado
                planoAcao.Atrasado = true;
                _uow.PlanoAcaoRepository.Update(planoAcao);
            }

            _uow.SaveChanges();
        }

        public void EncerrarResolvidos()
        {
            //consulta os planos de acao que estao com o status de resolvido a mais de 7 dias
            var planosAcao =
                _uow.PlanoAcaoRepository.Get(p =>
                p.Situacao == SituacaoPlanoAcao.RESOLVIDO &&
                p.DataConclusao < DateTime.Today.AddDays(-7));

            DateTime dataExecucao = DateTime.Now;

            foreach (PlanoAcao planoAcao in planosAcao)
            {
                Historico historicoS = GetModificacaoAutomatica(NomeCampoPlanoAcao.Situacao, planoAcao, dataExecucao);
                //HistoricoPlanoAcao historicoS = new HistoricoPlanoAcao
                //{
                //    DataModificacao = dataExecucao,
                //    Descricao = "Situação do plano de ação marcado como encerrado pelo sistema.",
                //    NomeCampo = NomeCampoPlanoAcao.Situacao,
                //    PlanoAcao = planoAcao,
                //    Tipo = TipoHistorico.Automatico,
                //    ValorAntigo = "Resolvido",
                //    ValorNovo = "Encerrado"
                //};

                planoAcao.Historicos.Add(historicoS);
                _uow.HistoricoRepository.Insert(historicoS);

                Historico historicoD = GetModificacaoAutomatica(NomeCampoPlanoAcao.Situacao, planoAcao, dataExecucao);
                //HistoricoPlanoAcao historicoD = new HistoricoPlanoAcao
                //{
                //    DataModificacao = dataExecucao,
                //    Descricao = "Data do plano de ação alterada pelo sistema.",
                //    NomeCampo = NomeCampoPlanoAcao.DataConclusao,
                //    PlanoAcao = planoAcao,
                //    Tipo = TipoHistorico.Automatico,
                //    ValorAntigo = planoAcao.DataConclusao.ToString(),
                //    ValorNovo = dataExecucao.ToString()
                //};

                planoAcao.Historicos.Add(historicoD);
                _uow.HistoricoRepository.Insert(historicoD);

                //marca os planos de acao como encerrado e salva
                planoAcao.Situacao = SituacaoPlanoAcao.ENCERRADO;
                planoAcao.DataConclusao = DateTime.Now;
                _uow.PlanoAcaoRepository.Update(planoAcao, true);
            }

            _uow.SaveChanges();
        }

        public static Historico GetModificacaoAutomatica(NomeCampoPlanoAcao nomeCampo, PlanoAcao planoAcao)
        {
            return GetModificacaoAutomatica(nomeCampo, planoAcao, DateTime.Now);
        }

        public static Historico GetModificacaoAutomatica(NomeCampoPlanoAcao nomeCampo, PlanoAcao planoAcao, DateTime dataModificacao)
        {
            string descricao = "";
            string valorAntigo = "";
            string valorNovo = "";

            switch (nomeCampo)
            {
                case NomeCampoPlanoAcao.Atrasado:
                    descricao = "Plano de ação marcado como atrasado pelo sistema.";
                    valorAntigo = "Falso";
                    valorNovo = "Verdadeiro";
                    break;

                case NomeCampoPlanoAcao.DataConclusao:
                    descricao = "Data do plano de ação alterada pelo sistema.";
                    valorAntigo = planoAcao.DataConclusao.ToString();
                    valorNovo = dataModificacao.ToString();
                    break;

                case NomeCampoPlanoAcao.Situacao:
                    descricao = "Situação do plano de ação marcado como encerrado pelo sistema.";
                    valorAntigo = "Resolvido";
                    valorNovo = "Encerrado";
                    break;
            }

            return new Historico
            {
                DataModificacao = dataModificacao,
                Descricao = descricao,
                NomeCampo = NomeCampoPlanoAcao.Situacao,
                PlanoAcao = planoAcao,
                Tipo = TipoHistorico.Automatico,
                ValorAntigo = valorAntigo,
                ValorNovo = valorNovo
            };
        }
    }
}