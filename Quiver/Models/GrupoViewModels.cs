using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class GrupoVM
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public List<ClassificacaoViewModels> ListaClassificacoes { get; set; }

        public static List<string> CorClassificacao
        {
            get
            {
                return new List<string>()
                {
                    // Do verdo ao vermelho
                    "339933",
                    "43c043",
                    "99cc33",
                    "ffc20d",
                    "ffcc33",
                    "ff9933",
                    "ff8a15",
                    "ff3333",
                    "ed2124",
                    "d91c1a"
                };
            }
        }
    }
}