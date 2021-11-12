using Quiver.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Avaliacao
{
    public class EventoDTO
    {
        public int Id { get; set; }

        public bool Agendada { get; set; }

        public bool? Conforme { get; set; }

        public String Titulo { get; set; }

        public DateTime Data { get; set; }

        public SituacaoAvaliacao Situacao { get; set; }
    }
}
