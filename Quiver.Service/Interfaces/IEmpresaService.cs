using Microsoft.AspNet.Identity;
using Quiver.DTO.Empresa;
using System.Collections;
using System.Collections.Generic;

namespace Quiver.Service.Interfaces
{
    public interface IEmpresaService
    {
        /// <summary>
        /// Obtém empresa por Id.
        /// </summary>
        /// <param name="IdEmpresa">Id da empresa</param>
        /// <returns>Retorna EmpresaDTO</returns>
        EmpresaDTO GetById(int IdEmpresa);

        /// <summary>
        /// Obtém empreas que contenham o termo.
        /// </summary>
        /// <param name="termo">Termo</param>
        /// <returns>Lista de empresas</returns>
        IList<EmpresaDTO> GetStartWithNome(string termo);

        /// <summary>
        /// Insere uma empresa.
        /// </summary>
        /// <param name="empresaToInsert">Empresa à inserir.</param>
        /// <returns>Id da empresa criada.</returns>
        int Insert(EmpresaDTO empresaToInsert);

        /// <summary>
        /// Insere uma empresa e um usuário do tipo Gestor.
        /// </summary>
        /// <param name="criarEmpresaDTO">Empresa e usuário a inserir.</param>
        /// <returns>Id da empresa criada.</returns>
        IdentityResult Insert(CriarEmpresaDTO criarEmpresaDTO);

        /// <summary>
        /// Atualiza as informações da empresa.
        /// </summary>
        /// <param name="empresaToUpdate">Id da empresa a ser atualizado.</param>
        void Update(EmpresaDTO empresaToUpdate);
    }
}
