using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.Core.Models
{
    public class Notificacao
    {
        public Notificacao()
        {
            
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UsuarioNotificado")]
        public string IdUsuarioNotificado { get; set; }

        [Required]
        [ForeignKey("Avaliacao")]
        public int IdAvaliacao { get; set; }

        public bool Lida { get; set; }

        public DateTime Data { get; set; }

        public TipoNotificacao Tipo { get; set; }

        public virtual Usuario UsuarioNotificado { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }
    }

    public enum TipoNotificacao
    {
        AVALIADO,
        ATRASADO
    }
}