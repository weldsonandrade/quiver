using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class PlanoAcao
    {

        public PlanoAcao()
        {
            this.Historicos = new HashSet<Historico>();
            this.Mensagens = new HashSet<Mensagem>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(500)]
        public string OQue { get; set; }

        [StringLength(500)]
        public string Porque { get; set; }

        [StringLength(100)]
        public string Quem { get; set; }

        [StringLength(500)]
        public string Como { get; set; }

        public decimal Quanto { get; set; }

        public DateTime? Quando { get; set; }

        [StringLength(200)]
        public string Onde { get; set; }

        [StringLength(100)]
        public string Responsavel { get; set; }

        public bool Atrasado { get; set; }

        public DateTime? DataConclusao { get; set; }

        [StringLength(500)]
        public string Justificativa { get; set; }

        [Required]
        public SituacaoPlanoAcao Situacao { get; set; }

        public virtual RespostaItem RespostaItem { get; set; }

        public virtual ICollection<Historico> Historicos { get; set; }

        public virtual ICollection<Mensagem> Mensagens { get; set; }

    }

    public enum SituacaoPlanoAcao
    {
        A_EDITAR,
        ANDAMENTO,
        RESOLVIDO,
        ENCERRADO,
        CANCELADO
    }
}
