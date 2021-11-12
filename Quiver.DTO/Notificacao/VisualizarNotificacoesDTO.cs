using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Notificacao
{
    public class VisualizarNotificacoesDTO
    {
        public int QuantidadeNaoLida { get; set; }

        public IList<NotificacaoDTO> Notificacoes { get; set; }
    }
}
