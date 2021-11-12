using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Models
{
    public class AvaliacaoQuestionarioGrupoDTO
    {
        public AvaliacaoQuestionarioGrupoDTO()
        {
            this.respostas = new List<RespostaDTO>();
        }

        public int IdQuestionario;
        public int IdAvalicao;
        public int IdGrupo;

        public List<RespostaDTO> respostas;
    }
}
