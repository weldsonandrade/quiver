using Microsoft.AspNet.Identity;
using Quiver.DTO.Perfil;
using Quiver.DTO.Usuario;
using Quiver.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtém usuarios ativos com perfil inspetor filtrado por empresa.
        /// </summary>
        /// <param name="IdEmpresa">Id da empresa</param>
        /// <returns></returns>
        IList<UsuarioDTO> GetAtivosByEmpresa(int IdEmpresa);

        /// <summary>
        /// Obtém usuário pelo Id.
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <returns>UsuarioDTO</returns>
        UsuarioDTO GetById(string idUsuario);

        /// <summary>
        /// Obtém usuários.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="termo">termo filtrado</param>
        /// <returns>UsuarioDTO</returns>
        IList<UsuarioDTO> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo);

        /// <summary>
        /// Obtém perfis.
        /// </summary>
        /// <returns>Lista dos perfis</returns>
        IList<PerfilDTO> GetPerfis();

        /// <summary>
        /// Obtém perfis exceto o de administrador.
        /// </summary>
        /// <returns>Lista dos perfis</returns>
        IList<PerfilDTO> GetPerfisExcetoAdministrador();

        /// <summary>
        /// Obté perfil gestor.
        /// </summary>
        /// <returns>Perfil Gestor</returns>
        PerfilDTO GetPerfilGestor();

        /// <summary>
        /// Obtém perfil por id.
        /// </summary>
        /// <returns>Perfil</returns>
        PerfilDTO GetPerfilById(string idPerfil);

        /// <summary>
        /// Exclui logicamente o usuário.
        /// </summary>
        /// <param name="usuarioToDelete">Id do usuário a ser deletada.</param>
        void Delete(string usuarioToDelete);

        /// <summary>
        /// Insere um usuário.
        /// </summary>
        /// <param name="usuarioToInsert">CriarUsuarioDTO para inserir.</param>
        IdentityResult Insert(CriarUsuarioDTO criarUsuario);
    }
}
