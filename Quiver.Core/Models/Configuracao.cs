using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    /// <summary>
    /// Responsável pelas configurações dependente de cada ambiente.
    /// </summary>
    public class Configuracao
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Index("IX_Nome_Unique", IsUnique = true)]
        public String Nome { get; set; }

        [Required]
        [StringLength(200)]
        public String Valor { get; set; }
    }
}
