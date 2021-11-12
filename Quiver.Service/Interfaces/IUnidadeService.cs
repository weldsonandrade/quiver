using Quiver.Core.Models;
using Quiver.DTO.Unidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IUnidadeService
    {
        /// <summary>
        /// Obtém unidade.
        /// </summary>
        /// <param name="IdUnidade"></param>
        /// <returns></returns>
        UnidadeDTO GetById(int IdUnidade);


        /// <summary>
        /// Obtém unidade.
        /// </summary>
        /// <param name="IdUnidade"></param>
        /// <returns></returns>
        UnidadeDTO GetByIdAndEmpresa(int IdUnidade, int IdEmpresa);

        /// <summary>
        /// Obtém unidades com Excluido == false e filtrado por empresa.
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        IList<UnidadeDTO> GetAtivosByEmpresa(int IdEmpresa);

        /// <summary>
        /// Obtém unidades com Excluido == false, filtrado por empresa e que começe com o nome.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="termo">Termo a ser filtrado.</param>
        /// <returns></returns>
        IList<UnidadeDTO> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo);

        /// <summary>
        /// Insere uma unidade.
        /// </summary>
        /// <param name="unidadeToInsert"></param>
        void Insert(UnidadeDTO unidadeToInsert);

        /// <summary>
        /// Atualiza as informações da unidade.
        /// </summary>
        /// <param name="unidadeToUpdate">Id da unidade a ser atualizada.</param>
        void Update(UnidadeDTO unidadeToUpdate);

        /// <summary>
        /// Exclui logicamente a unidade e todas as avaliações com status NAO_AVALIADO.
        /// </summary>
        /// <param name="unidadeToDelete">Id da unidade a ser deletada.</param>
        void Delete(int unidadeToDelete);

        /// <summary>
        /// Obtém id da empresa pertencente à unidade.
        /// </summary>
        /// <param name="IdUnidade">Ida da unidade</param>
        /// <returns>Id da Empresa da Uniadde</returns>
        int GetIdEmpresaOfUnidade(int IdUnidade);
    }
}
