using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class UnidadeVM
    {
        public int? Id { get; set; }

        public string Nome { get; set; }

        public int IdEmpresa { get; set; }
    }


    public class UnidadePerfilVM
    {
        public string Id { get; set; }

        public string IdEmpresa { get; set; }

        public string Nome { get; set; }

        public IList<AvaliacaoVM> avaliacoes { get; set; }

    }
}