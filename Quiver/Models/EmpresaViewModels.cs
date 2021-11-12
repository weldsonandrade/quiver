using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class EmpresaRowVM
    {
        public int Id { get; set; }

        public string Cnpj { get; set; }

        public string Nome { get; set; }

        public string Situacao { get; set; }

        public string LimiteLicencas { get; set; }
    }

    public class EmpresaVM
    {
        public int? Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cnpj { get; set; }

        public int Licencas { get; set; }
    }

    public class EmpresaEditorVM
    {
        public int? Id { get; set; }

        [StringLength(14, ErrorMessage = "A {0} deve ter pelo menos {2} de tamanho.", MinimumLength = 14)]
        [Required]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }

        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} de tamanho.", MinimumLength = 3)]
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Situação")]
        [MapTo("SituacaoEmpresa")]
        public bool Situacao { get; set; }

        //[MaxLength(10000, ErrorMessage = "Máximo de 10000.")]
        [Required]
        [Display(Name = "Limite de Licenças")]
        public int LimiteLicencas { get; set; }
    }
}