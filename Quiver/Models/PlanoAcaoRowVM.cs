using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class PlanoAcaoRowVM
    {
        public int Id { get; set; }

        public string OQue { get; set; }

        public string Porque { get; set; }

        public string Quem { get; set; }

        public string Como { get; set; }

        public decimal Quanto { get; set; }

        public DateTime Quando { get; set; }

        public string Onde { get; set; }

        public SituacaoPlanoAcao Situacao { get; set; }
    }
}