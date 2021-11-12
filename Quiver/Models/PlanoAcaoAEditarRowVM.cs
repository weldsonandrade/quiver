using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class PlanoAcaoAEditarRowVM
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Avaliacao { get; set; }

        public string Item { get; set; }

        public string Alternativa { get; set; }
    }
}