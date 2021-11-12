using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Models
{
    public class AvaliacaoDTO
    {
        public AvaliacaoDTO()
        {
            QuestionariosGrupo = new List<AvaliacaoQuestionarioGrupoDTO>();
        }

        public int Id { get; set; }

        public DateTime DataProgramada { get; set; }

        public int IdUnidade { get; set; }

        public List<AvaliacaoQuestionarioGrupoDTO> QuestionariosGrupo { get; set; }

        public bool IsLocal { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Dispositivo { get; set; }

        public string NomeResponsavel { get; set; }

        public string CargoResponsavel { get; set; }

        public string Assinatura { get; set; }

        public string Observacao { get; set; }
    }
}
