using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Classificacao
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        [Required]
        public int InicioIntervalo { get; set; }

        [Required]
        public int FimIntervalo { get; set; }

        [Required]
        [ForeignKey("Grupo")]
        public int IdGrupo { get; set; }

        [Required]
        [StringLength(6)]
        public string Cor { get; set; }

        public virtual Grupo Grupo { get; set; }
    }
}