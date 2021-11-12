using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class PlanoAcaoCardVM
    {
        public IList<PlanoAcaoAEditarRowVM> PlanosAcaoAEditar { get; set; }

        public IList<PlanoAcaoRowVM> PlanosAcaoAtrasados { get; set; }

        public IList<PlanoAcaoRowVM> PlanosAcaoAndamento { get; set; }

        public IList<PlanoAcaoRowVM> PlanosAcaoResolvidos { get; set; }

        public IList<PlanoAcaoRowVM> PlanosAcaoEncerrados { get; set; }

        public IList<PlanoAcaoRowVM> PlanosAcaoCancelados { get; set; }
    }
}