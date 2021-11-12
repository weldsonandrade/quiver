using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models.Enum
{
    public enum SituacaoPlanoAcao
    {
        [Display(Name = "Em andamento")]
        ANDAMENTO,
        [Display(Name = "Encerrado")]
        ENCERRADO,
        [Display(Name = "Cancelado")]
        CANCELADO
    }
}