using Quiver.Core.Models;
using System;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IAvaliacaoRepository : IRepository<Avaliacao>
    {
        IEnumerable<Avaliacao> GetByUnidadeAndSituacao(int unidadeId, SituacaoAvaliacao situacao);

        IEnumerable<Avaliacao> GetByGrupoAndSituacao(int grupoId, SituacaoAvaliacao situacao);

        Avaliacao GetById(int avaliacaoId);

        Avaliacao GetAvaliadaById(int avaliacaoId);

        IEnumerable<Avaliacao> GetByEmpresaAndPeriodo(int idEmpresa, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetByEmpresaAndPeriodoAndUnidadeAndUsuario(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, string idUsuario);

        IEnumerable<Avaliacao> GetByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetFinalizadasByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetFinalizadasComPontosByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetFinalizadasByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetFinalizadasComPontosByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetFinalizadasByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetFinalizadasComPontosByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal);

        IEnumerable<Avaliacao> GetAtrasadasByEmpresa(int idEmpresa);

        IEnumerable<Avaliacao> GetEmAndamentoByEmpresa(int idEmpresa);

        IEnumerable<Avaliacao> GetByEmpresaAndStartWithRotulo(int idEmpresa, string termo);

        IEnumerable<Avaliacao> GetByUsuario(string idUsuario);

        IEnumerable<Avaliacao> GetByUnidade(int idUnidade);

        IEnumerable<Avaliacao> GetByEmpresa(int idEmpresa);

        /// <summary>
        /// Retorna avaliações que contém o questionário.
        /// </summary>
        /// <param name="idQuestionario">Id do Questionário.</param>
        /// <returns>Lista de Avaliações.</returns>
        IEnumerable<Avaliacao> GetNaoAvaliadasByQuestionario(int idQuestionario);

        int GetIdEmpresaById(int idAvaliacao);

        /// <summary>
        /// Obtém avaliações que atenda aos critérios.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa, obrigatório</param>
        /// <param name="dataInicial">Data inicial, obrigatório</param>
        /// <param name="dataFinal">Data final, obrigatório</param>
        /// <param name="idUnidade">Id da Unidade, opcional</param>
        /// <param name="idUsuario">Id do usuário, opcional</param>
        /// <param name="idGrupo">Id do grupo, opcional</param>
        /// <param name="apenasAgendadas">True para apenas agendas, false para apenas não agendadas e NULL não usar este filtro.</param>
        /// <param name="apenasConformes">True para apenas conformes, false para apenas não conforme e NULL para não usar este filtro</param>
        /// <returns></returns>
        IEnumerable<Avaliacao> GetByFilter(int idEmpresa, DateTime dataInicial, DateTime dataFinal, 
            int idUnidade, string idUsuario, int idGrupo, bool? apenasAgendadas, bool? apenasConformes);

        IEnumerable<Avaliacao> GetAvaliadasByQuestionario(int idQuestionario);
    }
}
