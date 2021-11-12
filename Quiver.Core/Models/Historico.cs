using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public class Historico
    {
        public Historico() { }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DataModificacao { get; set; }

        [Required]
        public NomeCampoPlanoAcao NomeCampo { get; set; }

        [Required]
        [StringLength(500)]
        public String ValorAntigo { get; set; }

        [Required]
        [StringLength(500)]
        public String ValorNovo { get; set; }

        [StringLength(500)]
        public String Descricao { get; set; }

        [ForeignKey("PlanoAcao")]
        public int IdPlanoAcao { get; set; }

        [Required]
        public TipoHistorico Tipo { get; set; }

        [ForeignKey("Usuario")]
        public string IdUsuario { get; set; }

        public virtual PlanoAcao PlanoAcao { get; set; }

        public virtual Usuario Usuario { get; set; }
    }

    public enum TipoHistorico
    {
        Automatico,
        Manual
    }

    public enum NomeCampoPlanoAcao
    {
        OQue,
        Porque,
        Quem,
        Como,
        Quanto,
        Quando,
        Onde,
        Responsavel,
        Atrasado,
        DataConclusao,
        Situacao
    }
}
