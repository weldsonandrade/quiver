using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public class RespostaItem
    {

        public RespostaItem() { }

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Resposta")]
        public int IdResposta { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Item")]
        public int IdItem { get; set; }

        public Boolean NaoConformidade()
        {
            if (this.Item.Alternativa != null && this.Item.Alternativa.NaoConformidade == true)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public virtual Resposta Resposta { get; set; }

        public virtual Item Item { get; set; }

        public virtual PlanoAcao PlanoAcao { get; set; }
    }
}
