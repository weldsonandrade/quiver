using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiver.DTO.Notificacao;
using Quiver.Core.Models;
using Quiver.Service.Mappers;

namespace Quiver.Service.Implementation
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly IUnitOfWork _uow;

        public NotificacaoService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void Delete(IEnumerable<int> ids)
        {
            foreach(int id in ids)
            {
                _uow.NotificacaoRepository.Delete(id);
            }
            _uow.SaveChanges();
        }

        public IList<NotificacaoDTO> GetByEmpresaAndEmailUsuarioStartWithNome(int idEmpresa, string startWithEmail)
        {
            IEnumerable<Notificacao> notificacoes = _uow.NotificacaoRepository.GetByEmpresaAndEmailUsuarioStartWithNome(idEmpresa, startWithEmail);
            return NotificacaoMapper.MapNotificacaoToNotificacaoDTO(notificacoes);
        }

        public int GetQuantidadeNaoLidaByUsuario(string idUsuario)
        {
            return _uow.NotificacaoRepository.GetQuantidadeNaoLidaByUsuario(idUsuario);
        }

        public VisualizarNotificacoesDTO VisualizarNotificacoes(string idUsuario)
        {
            IEnumerable<Notificacao> notificacoes = _uow.NotificacaoRepository.GetAsCincoMaisRecentesByUsuario(idUsuario);

            // Atualizando as não visualizadas para visualizadas.
            //IEnumerable<Notificacao> naoLidas = notificacoes.Where(n => !n.Lida);
            IEnumerable<Notificacao> naoLidas = _uow.NotificacaoRepository.GetAsNaoLidasByUsuario(idUsuario);
            foreach (Notificacao notificacao in naoLidas)
            {
                notificacao.Lida = true;
                _uow.NotificacaoRepository.Update(notificacao);
            }
          
            if (naoLidas.Count() > 0)
            {
                _uow.SaveChanges();
            }

            return NotificacaoMapper.MapNotificacaoToVisualizarNotificacoesDTO(notificacoes);
        }
    }
}
