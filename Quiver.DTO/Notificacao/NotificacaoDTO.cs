using Quiver.DTO.Avaliacao;
using Quiver.DTO.Enum;
using Quiver.DTO.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Notificacao
{
    public class NotificacaoDTO
    {
        public int Id { get; set; }

        public string IdUsuarioNotificado { get; set; }

        public int IdAvaliacao { get; set; }

        public bool Lida { get; set; }

        public DateTime Data { get; set; }

        public UsuarioDTO UsuarioNotificado { get; set; }

        public AvaliacaoDTO Avaliacao { get; set; }

        public TipoNotificacao Tipo { get; set; }
    }
}
