using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Quiver.Core.Models
{
    public class Avaliacao
    {
        public Avaliacao()
        {
            this.QuestionariosAvaliacao = new HashSet<AvaliacaoQuestionarioGrupo>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DataProgramada { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public double LocalizacaoLatitude { get; set; }

        public double LocalizacaoLongitude { get; set; }

        [StringLength(100)]
        public string Dispositivo { get; set; }

        public int? PontuacaoMaxima { get; set; }

        public int? PontuacaoEfetuada { get; set; }

        // Base64
        public string Assinatura { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

        [StringLength(100)]
        public string RotuloCalendario { get; set; }

        [Required]
        [ForeignKey("Unidade")]
        public int IdUnidade { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public string IdUsuario { get; set; }

        [Required]
        public SituacaoAvaliacao Situacao { get; set; }

        [StringLength(100)]
        public string NomeResponsavel { get; set; }

        [StringLength(100)]
        public string CargoResponsavel { get; set; }

        public virtual ICollection<AvaliacaoQuestionarioGrupo> QuestionariosAvaliacao { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Unidade Unidade { get; set; }

        // Flag para descobrir quando a avaliação foi agendada previamente pelo gerente ou feita automáticamente pelo colaborador via mobile.
        public bool Agendada { get; set; }

        public virtual int Peso
        {
            get
            {
                return QuestionariosAvaliacao.Sum(q => q.Peso);
            }
        }

        // Prorpiedade usada para identificar todas as alterações que fazem parte de uma mesma serie
        public int? IdRecorrencia { get; set; }

        public bool? Conforme { get; set; }
    }

    public enum SituacaoAvaliacao
    {
        NAO_AVALIADO,
        AVALIADO
    }

    public enum TipoManipulacao
    {
        SomenteEsse,
        TodosRecorrentesFuturos,
        TodosRecorrentesNaoAvaliados
    }

    public enum FrequenciaRecorrencia
    {
        Diario,
        Semanal,
        Mensal,
        Customizado
    }
}