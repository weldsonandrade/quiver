using Quiver.DTO.AvaliacaoQuestionarioGrupo;
using Quiver.DTO.Grupo;
using Quiver.DTO.Questionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.QuestionarioGrupo
{
    public class QuestionarioGrupoDTO
    {
        public QuestionarioGrupoDTO()
        {
            this.Avaliacoes = new HashSet<AvaliacaoQuestionarioGrupoDTO>();
        }

        public int IdQuestionario { get; set; }

        public int IdGrupo { get; set; }

        public virtual QuestionarioDTO Questionario { get; set; }

        public virtual GrupoDTO Grupo { get; set; }

        public virtual ICollection<AvaliacaoQuestionarioGrupoDTO> Avaliacoes { get; set; }
    }
}
