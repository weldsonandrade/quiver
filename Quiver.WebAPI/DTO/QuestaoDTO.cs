using Quiver.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.WebAPI.DTO
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