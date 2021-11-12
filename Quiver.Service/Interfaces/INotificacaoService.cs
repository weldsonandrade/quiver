using Quiver.DTO.Notificacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface INotificacaoService
    {
        /// <summary>
        /// Obtém as notificações da empresa filtrado pelo usuario.
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <param name="startWithEmail">Começa com e-mail</param>
        /// <returns>Lista de notificações ordenadas da mais para a menos recente</returns>
        IList<NotificacaoDTO> GetByEmpresaAndEmailUsuarioStartWithNome(int idEmpresa, string startWithEmail);

        int GetQuantidadeNaoLidaByUsuario(string idUsuario);

        void Delete(IEnumerable<int> ids);

        VisualizarNotificacoesDTO VisualizarNotificacoes(string idUsuario);
    }
}
