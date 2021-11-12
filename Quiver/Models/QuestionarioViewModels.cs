using Quiver.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class QuestionarioVM
    {
        public QuestionarioVM()
        {
            this.Questoes = new List<QuestaoVM>();
        }

        public int? Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; }

        [Required]
        public int Responsavel { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "Ter no mínimo uma questão cadastrada")]
        public List<QuestaoVM> Questoes { get; set; }

    }

    public class QuestionarioRowVM
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int IdGrupo { get; set; }

        public string Grupos { get; set; }
    }



    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }

    public class QuestionarioGrupoVM
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public bool Marcado { get; set; }
    }
}