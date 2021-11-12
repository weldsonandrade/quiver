using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class OrigemNaoConformidadeVM
    {
        public string Rotulo { get; set; }

        public string Grupo { get; set; }

        public string Unidade { get; set; }

        public string Usuario { get; set; }

        public string Formulario { get; set; }

        public string Item { get; set; }

        public DateTime DataExecucao { get; set; }

        public List<AlternativaRespondidaVM> Alternativas { get; set; }
    }
}