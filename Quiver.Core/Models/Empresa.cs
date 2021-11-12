using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Empresa
    {
        public Empresa()
        {
            this.Grupos = new HashSet<Grupo>();
            this.Unidades = new HashSet<Unidade>();
            this.Usuarios = new HashSet<Usuario>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Icone { get; set; }

        [Required]
        [Range(0,100000)]
        [DefaultValue(10)]
        public int LimiteLicencas { get; set; }

        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; }

        [Required]
        public SituacaoEmpresa Situacao { get; set; }

        public virtual ICollection<Grupo> Grupos { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }

        public virtual ICollection<Unidade> Unidades { get; set; }

        public virtual ICollection<Questionario> Questionarios { get; set; }
    }

    public enum SituacaoEmpresa
    {
        DESATIVA,
        ATIVA
    }
}
