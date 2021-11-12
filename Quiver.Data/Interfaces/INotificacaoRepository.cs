using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Interfaces
{
    public interface INotificacaoRepository : IRepository<Notificacao>
    {
        /// <summary>
        /// Obtém as notificações da empresa filtrado pelo usuario.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="startWithEmail">Começa com e-mail</param>
        /// <returns>Lista de notificações ordenadas da mais para a menos recente</returns>
        IEnumerable<Notificacao> GetByEmpresaAndEmailUsuarioStartWithNome(int idEmpresa, string startWithEmail);

        int GetQuantidadeNaoLidaByUsuario(string idUsuario);

        /// <summary>
        /// Obtém as cinco notificações mais recentes do usuário.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário</param>
        /// <returns>Lista com as cinco Notificações mais recentes</returns>
        IEnumerable<Notificacao> GetAsCincoMaisRecentesByUsuario(string idUsuario);

        /// <summary>
        /// Obtém as notificações não lidas.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário</param>
        /// <returns>Lista com as Notificações não lidas</returns>
        IEnumerable<Notificacao> GetAsNaoLidasByUsuario(string idUsuario);
    }
}
