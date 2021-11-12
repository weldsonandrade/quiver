using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class AvaliacaoVM
    {
        public int? Id { get; set; }

        public string RotuloCalendario { get; set; }


        public string NomeUnidade { get; set; }


        public string NomeGrupo { get; set; }


        public string NomeUsuario { get; set; }

        public string EmailUsuario { get; set; }

        public DateTime DataProgramada { get; set; }

        public DateTime? DataExecutada { get; set; }

        public string ObservacaoAvaliacao { get; set; }

        public double LocalizacaoLatitude { get; set; }

        public double LocalizacaoLongitude { get; set; }

        public string Dispositivo { get; set; }

        public int? PontuacaoMaxima { get; set; }

        public int? PontuacaoEfetuada { get; set; }

        public string Assinatura { get; set; }

        public string Situacao { get; set; }

        public bool Conforme { get; set; }

        public bool Agendada { get; set; }


    }


    public class AvaliacaoGraficoVM
    {

        public long DataExecutada { get; set; }
        public DateTime DataFinal { get; set; }
        public double Efetividade { get; set; }
        public string RotuloCalendario { get; set; }

    }

    public class AvaliacoesPorDiaGraficoVM
    {

        public long DataExecutada { get; set; }
        public DateTime DataFinal { get; set; }

        public int Quantidade { get; set; }

    }

    public class quantitativoAvaliacoesDashboard
    {
        public int QtdTotal { get; set; }
        public string Evetividade { get; set; }
        public int QtdFinalizadas { get; set; }
        public int QtdAndamentos { get; set; }
        public int QtdAtrasadas { get; set; }
        public int QtdComNaoConformidade { get; set; }
        public int QtdConformes { get; set; }
        public int QtdNaoAgendadas { get; set; }
        public int QtdAgendadas { get; set; }
    }


}