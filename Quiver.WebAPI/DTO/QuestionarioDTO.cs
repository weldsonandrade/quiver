using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quiver.Core.Models;
using System.Linq;

namespace Quiver.WebAPI.DTO
{
    public class QuestionarioDTO
    {
        public QuestionarioDTO()
        {
            Questoes = new List<QuestaoDTO>();
        }

        public int Id { get; set; }

        public int Ordem { get; set; }

        public string Nome { get; set; }

        public List<QuestaoDTO> Questoes { get; set; }
    }
}