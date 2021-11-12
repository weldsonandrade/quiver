using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Foto
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        // Base64
        public string Fotografia { get; set; }

        [Required]
        [ForeignKey("Resposta")]
        public int IdResposta { get; set; }

        public virtual Resposta Resposta { get; set; }
    }
}