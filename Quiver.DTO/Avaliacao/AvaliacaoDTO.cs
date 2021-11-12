using Quiver.DTO.AvaliacaoQuestionarioGrupo;
using Quiver.DTO.Enum;
using Quiver.DTO.Unidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Avaliacao
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

        public int IdEmpresa { get; set; }

        public string IdUsuario { get; set; }

        public string NomeUsuario { get; set; }

        public string EmailUsuario { get; set; }

        public int IdGrupo { get; set; }

        public List<AvaliacaoQuestionarioGrupoDTO> QuestionariosGrupo { get; set; }

        //public bool IsLocal { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Dispositivo { get; set; }

        public string NomeResponsavel { get; set; }

        public string CargoResponsavel { get; set; }

        public string Assinatura { get; set; }

        public string Observacao { get; set; }

        public string RotuloCalendario { get; set; }

        public UnidadeDTO Unidade { get; set; }

        public SituacaoAvaliacao Situacao { get; set; }

        public bool Agendada { get; set; }

        public bool? Conforme { get; set; }

        public int PontuacaoMaxima { get; set; }

        public int PontuacaoEfetuada { get; set; }
    }
}
