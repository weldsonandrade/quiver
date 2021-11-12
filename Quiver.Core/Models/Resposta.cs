using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Resposta
    {
        public Resposta()
        {
            this.Fotos = new HashSet<Foto>();
            this.Itens = new HashSet<RespostaItem>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(500)]
        public string Justificativa{ get; set; }

        [Required]
        public int Pontos { get; set; }


        [Required]
        [Column(Order = 0)]
        [ForeignKey("AvaliacaoQuestionarioGrupo")]
        public int IdAvaliacao { get; set; }

        [Required]
        [Column(Order = 1)]
        [ForeignKey("AvaliacaoQuestionarioGrupo")]
        public int IdQuestionario { get; set; }

        [Required]
        [Column(Order = 2)]
        [ForeignKey("AvaliacaoQuestionarioGrupo")]
        public int IdGrupo { get; set; }

        public virtual ICollection<RespostaItem> Itens { get; set; }

        public virtual ICollection<Foto> Fotos { get; set; }

        public virtual AvaliacaoQuestionarioGrupo AvaliacaoQuestionarioGrupo { get; set; }
    }
}