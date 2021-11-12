using Quiver.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class PlanoAcaoVM
    {
        // Propriedades da inspeção e resposta que originou a não conformidade.
        public OrigemNaoConformidadeVM Origem { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string OQue { get; set; }

        [Required]
        [StringLength(500)]
        public string Porque { get; set; }

        [Required]
        [StringLength(100)]
        public string Quem { get; set; }

        [Required]
        [StringLength(500)]
        public string Como { get; set; }

        [Required]
        public decimal Quanto { get; set; }

        [Required]
        public DateTime Quando { get; set; }

        [Required]
        [StringLength(200)]
        public string Onde { get; set; }

        [Required]
        [StringLength(100)]
        public string Responsavel { get; set; }

        public string Observaocao { get; set; }

        [Required]
        public SituacaoPlanoAcao Situacao { get; set; }

    }
}