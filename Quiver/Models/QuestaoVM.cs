using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class QuestaoVM
    {
        public int? Id { get; set; }

        [StringLength(1000)]
        public string Descricao { get; set; }

        public int Ordem { get; set; }


        public bool ExigeJustificativa { get; set; }

        public TipoQuestao Tipo { get; set; }
        
        public bool ExigeResposta { get; set; }

        public Questionario Questionario { get; set; }

        public List<AlternativaVM> Alternativas { get; set; }

        public QuestaoVM()
        {

        }
    }

    public class AlternativaVM
    {
        public int Id { get; set; }

        [StringLength(1000)]
        public string Descricao { get; set; }

        public int Peso { get; set; }

        public int Ordem { get; set; }

        public bool NaoConformidade { get; set; }

        public bool ExigeJustificativa { get; set; }
    }
    
}