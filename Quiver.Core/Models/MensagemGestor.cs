using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public class MensagemGestor : Mensagem
    {
        [ForeignKey("Usuario")]
        public virtual string IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }        
    }
}
