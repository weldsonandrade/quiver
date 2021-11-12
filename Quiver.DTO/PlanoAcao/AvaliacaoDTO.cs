using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.PlanoAcao
{
    public class AvaliacaoDTO
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public string Rotulo { get; set; }

        public string Unidade { get; set; }

        public string Usuario { get; set; }

        public DateTime DataFim { get; set; }
    }
}
