using Quiver.DTO.Grupo;
using Quiver.DTO.Questao;
using Quiver.DTO.QuestionarioGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Questionario
{
    public class QuestionarioDTO
    {
        public QuestionarioDTO()
        {
            Questoes = new List<QuestaoDTO>();
        }

        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int Ordem { get; set; }

        public string Nome { get; set; }

        public bool Excluido { get; set; }

        public List<QuestionarioGrupoDTO> Grupos { get; set; }

        public List<QuestaoDTO> Questoes { get; set; }
    }
}
