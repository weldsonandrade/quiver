using Quiver.DTO.Grupo;
using Quiver.DTO.Questionario;
using Quiver.DTO.QuestionarioGrupo;
using Quiver.DTO.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.AvaliacaoQuestionarioGrupo
{
    public class AvaliacaoQuestionarioGrupoDTO
    {
        public AvaliacaoQuestionarioGrupoDTO()
        {
            this.Respostas = new List<RespostaDTO>();
        }

        public int IdQuestionario;
        public int IdAvaliacao;
        public int IdGrupo;
        public QuestionarioGrupoDTO QuestionarioGrupo;

        public List<RespostaDTO> Respostas;
    }
}
