using System;
using Quiver.DTO.PlanoAcao;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IPlanoAcaoService
    {
        /// <summary>
        ///  Obtém Plano de Ação.
        /// </summary>
        /// <param name="idPlanoAcao">id do plano.</param>
        /// <returns>Plano de ação.</returns>
        PlanoAcaoDTO GetById(int idPlanoAcao);

        /// <summary>
        /// Obtém os planos de ação por empresa.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa.</param>
        /// <returns>Lista de planos de ação.</returns>
        IList<PlanoAcaoDTO> GetByEmpresa(int idEmpresa);

        /// <summary>
        /// Obtém os planos de ação por período para uma empresa.
        /// </summary>
        /// <param param name="idEmpresa">Id da empresa.</param>
        /// <param name="dataInicial">Data Incial, se null não filtra por data inicial.</param>
        /// <param name="dataFinal">Data final, se null não filtra pela data final.</param>
        /// <returns>Lista de planos de ação que pertençam ao período.</returns>
        IList<PlanoAcaoDTO> GetByEmpresaAndPeriodo(int idEmpresa, Nullable<DateTime> dataInicial, Nullable<DateTime> dataFinal);

        /// <summary>
        /// Obtém os planos de ação que estejam dentro do filtro.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa é obrigatório.</param>
        /// <param name="dataInicial">Data Inicial, se null não filtra por data inicial.</param>
        /// <param name="dataFinal">Data final, se null não filtra pela data final.</param>
        /// <param name="emailResponsavel">E-mail do responsável, se não não filtra pelo e-mail.</param>
        /// <param name="unidades">Lista de Ids das unidades.</param>
        /// <param name="usuarios">Lista de Ids dos usuários.</param>
        /// <returns>Lista de planos de ação que contemplem o filtro.</returns>
        IList<PlanoAcaoDTO> GetByEmpresaAndFiltro(int idEmpresa, DateTime dataInicial, DateTime dataFinal, string emailResponsavel, List<int> unidades, List<int> usuarios);

        /// <summary>
        /// Obtém os planos de ação que estejam dentro do filtro.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa é obrigatório.</param>
        /// <param name="emailResponsavel">E-mail do responsável, se não não filtra pelo e-mail.</param>
        /// <param name="unidades">Lista de Ids das unidades.</param>
        /// <param name="usuarios">Lista de Ids dos usuários.</param>
        /// <returns>Lista de planos de ação que contemplem o filtro.</returns>
        IList<PlanoAcaoDTO> GetByEmpresaAndResponsavelAndUnidadesAndUsuarios(int idEmpresa, string emailResponsavel, List<int> unidades, List<string> usuarios);

        /// <summary>
        /// Altera as informaçõoes do Plano de ação.
        /// </summary>
        /// <param name="planoAcaoToUpdate">Plano de Ação a ser atualizado.</param>
        void Update(PlanoAcaoDTO planoAcaoToUpdate);

        /// <summary>
        /// Marcar um plano de ação como cancelado.
        /// </summary>
        /// <param name="idPlanoAcao">Id do plano de ação.</param>
        void Cancelar(int idPlanoAcao);
    }
}
