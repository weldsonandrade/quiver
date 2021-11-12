using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quiver.Core.Models;
using System.Web.Mvc;
using Quiver.DTO.Notificacao;

namespace Quiver.Models
{
    public class HeaderViewModel
    {
        public HeaderViewModel()
        {
            Notificacoes = new List<NotificacaoDTO>();
        }

        public int QuantidadeNaoLida { get; set; }

        public List<NotificacaoDTO> Notificacoes { get; set; }
    }


    public class NotificacoesVM 
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public TipoNotificacao Tipo { get; set; }

        public  Usuario UsuarioNotificado { get; set; }

        public  Avaliacao Avaliacao { get; set; }
    }
}