using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Alternativa
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Descricao { get; set; }

        [Required]
        public int Peso { get; set; }

        [Required]
        public int Ordem { get; set; }

        [Required]
        public bool NaoConformidade { get; set; }

        [Required]
        public bool ExigeJustificativa { get; set; }

    }
}