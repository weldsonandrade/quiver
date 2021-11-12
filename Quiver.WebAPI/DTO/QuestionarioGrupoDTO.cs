using System.Collections.Generic;

namespace Quiver.WebAPI.DTO
{
    public class QuestionarioGrupoDTO
    {
        public QuestionarioGrupoDTO()
        {
        }

        public int IdQuestionario { get; set; }

        public int IdGrupo { get; set; }

        public virtual QuestionarioDTO Questionario { get; set; }

        public virtual GrupoDTO Grupo { get; set; }
    }
}