using Quiver.DTO.Classificacao;
using Quiver.DTO.QuestionarioGrupo;
using System.Collections.Generic;

namespace Quiver.DTO.Grupo
{
    public class GrupoDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int IdEmpresa { get; set; }

        public List<ClassificacaoDTO> Classificacoes { get; set; }

        public List<QuestionarioGrupoDTO> Questionarios { get; set; }
    }
}
