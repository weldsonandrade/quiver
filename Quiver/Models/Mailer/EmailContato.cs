using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models.Mailer
{
    public class EmailContato
    {
      
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(64, ErrorMessage = "Limite de 64 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido, formato errado.")]
        [Display(Name = "E-mail")]
        [StringLength(128, ErrorMessage = "Limite de 128 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mensagem é obrigatório.")]
        [StringLength(4096, ErrorMessage = "Permitido até 4096 caracteres.")]
        public string Mensagem { get; set; }



        public EmailContato() { }

      
    }
}