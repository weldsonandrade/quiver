using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.PlanoAcao
{
    public class AlternativaDTO
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public bool Respondida { get; set; }

        public bool NaoConformidade { get; set; }
    }
}
