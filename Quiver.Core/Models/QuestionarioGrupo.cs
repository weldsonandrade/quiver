using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public class QuestionarioGrupo
    {
        public QuestionarioGrupo()
        {
            this.Avaliacoes = new HashSet<AvaliacaoQuestionarioGrupo>();
        }

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Questionario")]
        public int IdQuestionario { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Grupo")]
        public int IdGrupo { get; set; }

        public virtual Questionario Questionario { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual ICollection<AvaliacaoQuestionarioGrupo> Avaliacoes { get; set; }
    }
}
