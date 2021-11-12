using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.PlanoAcao
{
    public class QuestaoDTO
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public FormularioDTO Formulario { get; set; }

        public List<AlternativaDTO> Alternativas { get; set; }
    }
}
