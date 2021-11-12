using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Item
    {

        public Item()
        {
            this.Respostas = new HashSet<RespostaItem>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Alternativa")]
        public int? IdAlternativa { get; set; }

        [Required]
        [ForeignKey("Questao")]
        public int IdQuestao { get; set; }

        public virtual Alternativa Alternativa { get; set; }

        public virtual Questao Questao { get; set; }

        public virtual ICollection<RespostaItem> Respostas { get; set; }

        public virtual int Peso
        {
            get
            {
                return Alternativa != null ? Alternativa.Peso : 0;
            }
        }
    }
}