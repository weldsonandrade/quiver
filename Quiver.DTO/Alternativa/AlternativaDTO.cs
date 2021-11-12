using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Alternativa
{
    public class AlternativaDTO
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int Peso { get; set; }

        public int Ordem { get; set; }

        public bool NaoConformidade { get; set; }

        public bool ExigeJustificativa { get; set; }
    }
}
