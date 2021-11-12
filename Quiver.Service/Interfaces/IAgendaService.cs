using Quiver.DTO.Avaliacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IAgendaService
    {
       

        /// <summary>
        /// Obtém avaliação.
        /// </summary>
        /// <param name="idAvaliacao">Id da avaliação</param>
        /// <returns></returns>
        AvaliacaoDTO GetAvaliacaoById(int idAvaliacao);

        /// <summary>
        /// Obtém avaliações por empresa.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <returns>Lista de avaliações</returns>
        IEnumerable<AvaliacaoDTO> GetAvaliacoesByEmpresa(int idEmpresa);

        /// <summary>
        /// Obtém avaliações em andamento por empresa.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <returns>Lista de avaliações em andamento</returns>
        IEnumerable<AvaliacaoDTO> GetAvaliacoesEmAndamentoByEmpresa(int idEmpresa);

        /// <summary>
        /// Obtém avaliações atrasadas por empresa.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <returns>Lista de avaliações atrasadas</returns>
        IEnumerable<AvaliacaoDTO> GetAvaliacoesAtrasadasByEmpresa(int idEmpresa);

        /// <summary>
        /// Obtém avaliações.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="dataInicial">Inicio do perido (incluso)</param>
        /// <param name="dataFinal">Final do periodo (incluso)</param>
        /// <param name="idUnidade">Id da unidade, 0 para não filtrar por este campo</param>
        /// <param name="idUsuario">Id do usuario, "0" para não filtrar por este campo</param>
        /// <param name="idGrupo">Id do grupo, 0 para não filtrar por este campo.</param>
        /// <param name="apenasAgendadas">True para apenas agendas, false para apenas não agendadas e NULL não usar este filtro.</param>
        /// <param name="apenasConformes">True para apenas conformes, false para apenas não conforme e NULL para não usar este filtro</param>
        /// <returns></returns>
        IEnumerable<AvaliacaoDTO> GetAvaliacaoByFilter(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, 
            String idUsuario, int idGrupo, bool? apenasAgendadas, bool? apenasConformes);

        /// <summary>
        /// Obtém avaliações filtrada por empresa, período, unidade e usuário.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="dataInicial">Inicio do perido (incluso)</param>
        /// <param name="dataFinal">Final do periodo (incluso)</param>
        /// <param name="idUnidade">Id da Unidade, 0 para não pesqueisar por este campo.</param>
        /// <param name="idUsuario">Id do Usuário, vazio ou null para não pesquisar por este campo.</param>
        /// <returns>Lista de AvaliacoesDTO</returns>
        IList<EventoDTO> GetAvaliacaoByEmpresaAndPeriodoAndUnidadeAndUsuario(int idEmpresa, DateTime dataInicial, DateTime dataFinal, int idUnidade, string idUsuario);

        /// <summary>
        /// Obtém avaliações.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="dataInicial">Inicio do perido (incluso)</param>
        /// <param name="dataFinal">Final do periodo (incluso)</param>
        /// <returns></returns>
        IList<AvaliacaoDTO> GetAvaliacoesByEmpresaAndStartWithRotulo(int idEmpresa, string termo);



        /// <summary>
        /// Obtém avaliação com situação avaliada.
        /// </summary>
        /// <param name="idAvaliacao">Id da avaliação</param>
        /// <returns></returns>
        AvaliacaoDTO GetAvaliacaoAvaliadaById(int idAvaliacao);

        /// <summary>
        /// Insere uma avaliação.
        /// </summary>
        /// <param name="avaliacaoToInsert">Avaliação para inserir</param>
        void Insert(AvaliacaoDTO avaliacaoToInsert);

        /// <summary>
        /// Atualiza as informações da avaliação.
        /// </summary>
        /// <param name="avaliacaoToUpdate">Id da avaliação a ser atualizada.</param>
        void Update(AvaliacaoDTO avaliacaoToUpdate);

        /// <summary>
        /// Atualiza a data programada de uma avaliação que não foi avaliada.
        /// </summary>
        /// <param name="avaliacaoToUpdate">Id da avaliação a ser atualizada.</param>
        void UpdateDataProgramada(AvaliacaoDTO avaliacaoToUpdate);

        /// <summary>
        /// Exclui a avaliação.
        /// </summary>
        /// <param name="avaliacaoToDelete">Id da avaliação a ser deletada.</param>
        void Delete(int avaliacaoToDelete);

        /// <summary>
        /// Obtém avaliações filtrado pelo id do usuário
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <returns>Lista de avaliações ordenadas pela data programada.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesByUsuario(string idUsuario);

        /// <summary>
        /// Obtém avaliações filtrado pelo id da unidade
        /// </summary>
        /// <param name="idUnidade">Id da unidade</param>
        /// <returns>Lista de avaliações ordenadas pela data programada.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesByUnidade(int idUnidade);


        /// <summary>
        /// Obtém avaliações filtradas pelo usuário e período.
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações ordenadas pela data programada.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal);


        /// <summary>
        /// Obtém avaliações filtradas pela unidade e período de tempo.
        /// </summary>
        /// <param name="idUnidade">Id da unidade</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações ordenadas pela data programada.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesByUnidadeAndPeriodo(string idUnidade, DateTime dataInicial, DateTime dataFinal);



        /// <summary>
        /// Obtém avaliações finalizadas filtradas pelo usuário e período.
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações finalizadas ordenadas pela data Final.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesFinalizadasByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal);

        /// <summary>
        /// Obtém avaliações finalizadas filtradas pelo usuário e período.
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações finalizadas ordenadas pela data Final.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesFinalizadasComPontosByUsuarioAndPeriodo(string idUsuario, DateTime dataInicial, DateTime dataFinal);

        /// <summary>
        /// Obtém avaliações finalizadas filtradas pela unidade e período.
        /// </summary>
        /// <param name="idUnidade">Id da unidade</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações finalizadas.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesFinalizadasByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal);


        /// <summary>
        /// Obtém avaliações finalizadas com alguma pontuação filtradas pela unidade e período.
        /// </summary>
        /// <param name="idUnidade">Id da unidade</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações finalizadas.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesFinalizadasComPontosByUnidadeAndPeriodo(int idUnidade, DateTime dataInicial, DateTime dataFinal);


        /// <summary>
        /// Obtém avaliações finalizadas filtradas pelo grupo e período.
        /// </summary>
        /// <param name="idGrupo">Id do grupo</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações finalizadas.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesFinalizadasByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal);

        /// <summary>
        /// Obtém avaliações finalizadas que tenham uma pontuaçãofiltradas pelo grupo e período.
        /// </summary>
        /// <param name="idGrupo">Id do grupo</param>
        /// <param name="dataInicial">Data Inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Lista de Avaliações finalizadas.</returns>
        IList<AvaliacaoDTO> GetAvaliacoesFinalizadasComPontosByGrupoAndPeriodo(int idGrupo, DateTime dataInicial, DateTime dataFinal);

        /// <summary>
        /// Obtém id da empresa pertencente à avaliação.
        /// </summary>
        /// <param name="IdUnidade">Id da avaliação</param>
        /// <returns>Id da Empresa da Avaliação</returns>
        int GetIdEmpresaOfAvaliacao(int IdAvaliacao);
    }
}
