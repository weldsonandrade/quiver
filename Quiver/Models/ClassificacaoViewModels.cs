using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quiver.Core.Models;
using System.Web.Mvc;

namespace Quiver.Models
{
    public class ClassificacaoViewModels
    {
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public int InicioIntervalo { get; set; }

        public int FimIntervalo { get; set; }

        public string CorIntervaloClassificacao { get; set; }

        public IEnumerable<SelectListItem> ListaCoresIntervalo { get; set; }

        public Grupo Grupo { get; set; }
    }
}