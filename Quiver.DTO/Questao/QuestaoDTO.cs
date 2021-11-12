using Quiver.DTO.Alternativa;
using Quiver.DTO.Enum;
using Quiver.DTO.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Questao
{
    public class QuestaoDTO
    {
        public QuestaoDTO()
        {
            Itens = new List<ItemDTO>();
        }

        public int Id { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public bool ExigeJustificativa { get; set; }

        public TipoQuestao Tipo { get; set; }

        public bool ExigeResposta { get; set; }

        public List<ItemDTO> Itens { get; set; }
    }
}
