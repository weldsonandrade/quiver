using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Classificacao
{
    public class ClassificacaoDTO
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int InicioIntervalo { get; set; }

        public int FimIntervalo { get; set; }

        public int IdGrupo { get; set; }

        public string Cor { get; set; }
    }
}
