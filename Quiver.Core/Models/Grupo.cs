using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Grupo
    {
        public Grupo()
        {
            this.Questionarios = new HashSet<QuestionarioGrupo>();
            this.Usuarios = new HashSet<Usuario>();
            this.Classificacoes = new HashSet<Classificacao>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }

        [Required]
        public bool Excluido { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual ICollection<QuestionarioGrupo> Questionarios { get; set; }       

        public virtual ICollection<Usuario> Usuarios { get; set; }

        public virtual ICollection<Classificacao> Classificacoes { get; set; }
    }
}