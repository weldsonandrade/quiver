using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public abstract class Mensagem
    {
        public Mensagem() { }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public String Texto { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [ForeignKey("PlanoAcao")]
        public int IdPlanoAcao { get; set; }

        public virtual PlanoAcao PlanoAcao { get; set; }
    }
}
