using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.WebAPI.Models
{
    public class NotificacaoVM
    {
        public string Email { get; set; }
        public string Unidade { get; set; }
        public string Grupo { get; set; }
        public string DataProgramada { get; set; }
        public string Aplicador { get; set; }
    }
}