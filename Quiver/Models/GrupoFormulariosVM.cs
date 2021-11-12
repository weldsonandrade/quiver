using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Quiver.Models
{
    public class GrupoFormulariosVM 
    {
        [Required]
        public int Id { get; set; }

        public FormularioSelectedVM[] Formularios { get; set; }

    }
}
