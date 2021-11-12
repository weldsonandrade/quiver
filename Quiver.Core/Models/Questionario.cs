using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Quiver.Core.Models
{
    public class Questionario
    {
        public Questionario()
        {
            this.Grupos = new HashSet<QuestionarioGrupo>();
            this.Questoes = new HashSet<Questao>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Ordem { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; }

        [Required]
        public bool Excluido { get; set; }

        [Required]
        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }        

        [ForeignKey("QuestionarioAnterior")]
        public int? IdQuestionarioAnterior { get; set; }

        public virtual Questionario QuestionarioAnterior { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual ICollection<Questao> Questoes { get; set; }

        public virtual ICollection<QuestionarioGrupo> Grupos { get; set; }

        public virtual int Peso
        {
            get
            {
                return Questoes.Sum(q => q.Peso);
            }
        }
    }
}