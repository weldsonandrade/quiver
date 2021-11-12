using Quiver.DTO.Item;
using Quiver.DTO.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.RespostaItem
{
    public class RespostaItemDTO
    {
        public virtual RespostaDTO Resposta { get; set; }

        public virtual ItemDTO Item { get; set; }
    }
}
