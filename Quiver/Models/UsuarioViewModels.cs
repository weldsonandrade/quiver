using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace Quiver.Models
{
    public class UsuarioRow
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public string Perfil { get; set; }

        public bool Logado { get; set; }
    }

    public class UsuarioVM
    {
        public string Id { get; set; }

        public string IdEmpresa { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        [Required]
        public string Login { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; }

        [Required]
        public string Perfil { get; set; }

        public bool IsEditar { get; set; }
    }


    public class UsuarioPerfilVM
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public IList<AvaliacaoVM> avaliacoes { get; set; }

        public string Perfil { get; set; }
    }


}