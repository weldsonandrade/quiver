using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.PlanoAcao
{
    public class FormularioDTO
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public string Grupo { get; set; }

        public AvaliacaoDTO Avaliacao { get; set; }
    }
}
