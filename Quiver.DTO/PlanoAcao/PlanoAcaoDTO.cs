using Quiver.DTO.Avaliacao;
using Quiver.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.PlanoAcao
{
    public class PlanoAcaoDTO
    {
        public int Id { get; set; }

        public string OQue { get; set; }

        public string Porque { get; set; }

        public string Quem { get; set; }

        public string Como { get; set; }

        public decimal Quanto { get; set; }

        public DateTime Quando { get; set; }

        public string Onde { get; set; }

        public string Responsavel { get; set; }

        public bool Atrasado { get; set; }

        public string Justificativa { get; set; }

        public DateTime? DataConclusao { get; set; }

        public SituacaoPlanoAcao Situacao { get; set; }

        public QuestaoDTO Questao { get; set; }
    }
}
