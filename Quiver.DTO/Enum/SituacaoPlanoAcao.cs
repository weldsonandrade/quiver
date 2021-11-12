using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Enum
{
    public enum SituacaoPlanoAcao
    {
        [Description("A editar")]
        [Display(Name = "A editar")]
        A_EDITAR,
        [Description("Em andamento")]
        [Display(Name = "Em andamento")]
        ANDAMENTO,
        //[Description("Resolvido")]
        //RESOLVIDO,
        [Description("Encerrado")]
        [Display(Name = "Encerrado")]
        ENCERRADO,
        [Description("Cancelado")]
        [Display(Name = "Cancelado")]
        CANCELADO
    }
}
