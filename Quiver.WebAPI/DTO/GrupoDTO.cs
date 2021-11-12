using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.WebAPI.DTO
{
    public class GrupoDTO
    {
        public GrupoDTO()
        {
            QuestionariosGrupo = new List<QuestionarioGrupoDTO>();
        }

        public int Id { get; set; }

        public string Nome { get; set; }

        public List<QuestionarioGrupoDTO> QuestionariosGrupo { get; set; }
    }
}