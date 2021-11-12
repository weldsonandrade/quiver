using Quiver.DTO.Grupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IGrupoService
    {
        /// <summary>
        /// Obtém grupo.
        /// </summary>
        /// <param name="IdUnidade"></param>
        /// <returns></returns>
        GrupoDTO GetById(int IdGrupo);


        /// <summary>
        /// Obtém grupos com Excluido == false e filtrado por empresa.
        /// </summary>
        /// <param name="IdEmpresa">Id da empresa.</param>
        /// <returns>Os grupos ordenados pelo nome em ordem crescente.</returns>
        IList<GrupoDTO> GetAtivosByEmpresa(int IdEmpresa);

        /// <summary>
        /// Obtém grupos ativos, filtrado por empresa e que tenham pelo menos um questionário cadastrado.
        /// </summary>
        /// <param name="IdEmpresa">id da empresa</param>
        /// <returns></returns>
        IList<GrupoDTO> GetAtivosByEmpresaWithAtLeastOneQuestionario(int idEmpresa);

        /// <summary>
        /// Obtém grupo com Excluido == false, filtrado por empresa e que começe com o nome.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="termo">Termo a ser filtrado.</param>
        /// <returns></returns>
        IList<GrupoDTO> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo);

        /// <summary>
        /// Insere um grupo.
        /// </summary>
        /// <param name="grupoToInsert"></param>
        void Insert(GrupoDTO grupoToInsert);

        /// <summary>
        /// Atualiza as informações do grupo.
        /// </summary>
        /// <param name="grupoToUpdate">Id do grupo a ser atualizado.</param>
        void Update(GrupoDTO grupoToUpdate);

        /// <summary>
        /// Exclui logicamente o grupo com seus questionários e exclui todas as avaliações com status NAO_AVALIADO.
        /// </summary>
        /// <param name="grupoToDelete">Id do grupo a ser deletado.</param>
        void Delete(int grupoToDelete);
    }
}
