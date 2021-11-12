using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public class AvaliacaoQuestionarioGrupo
    {
        public AvaliacaoQuestionarioGrupo()
        {
            this.Respostas = new List<Resposta>();
        }

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Avaliacao")]
        public int IdAvaliacao { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("QuestionarioGrupo")]
        public int IdQuestionario { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("QuestionarioGrupo")]
        public int IdGrupo { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        public SituacaoAvaliacao Situacao { get; set; }

        public virtual QuestionarioGrupo QuestionarioGrupo { get; set; }

        public virtual ICollection<Resposta> Respostas { get; set; }

        public virtual int Peso
        {
            get
            {
                return QuestionarioGrupo.Questionario.Peso;
            }
        }

    }
}
