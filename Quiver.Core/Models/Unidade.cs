using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Unidade
    {
        public Unidade()
        {
            this.Avaliacoes = new HashSet<Avaliacao>();
        }

        public static Unidade FactoryInsert(string nome, int idEmpresa)
        {
            return new Unidade() { Nome = nome.Trim(), IdEmpresa = idEmpresa };
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public bool Excluido { get; set; }

        [Required]
        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        
        public virtual Empresa Empresa { get; set; }

        public virtual ICollection<Avaliacao> Avaliacoes { get; set; }

    }
}