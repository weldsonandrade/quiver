using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Models
{
    public class RespostaDTO
    {
        public RespostaDTO()
        {
            this.Fotos = new List<string>();
            this.Itens = new List<int>();
        }

        public string Justificativa { get; set; }

        public List<string> Fotos { get; set; }

        public List<int> Itens { get; set; }
    }
}
