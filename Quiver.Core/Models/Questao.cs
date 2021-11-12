using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Quiver.Core.Models
{
    public class Questao
    {
        public Questao()
        {
            this.Itens = new HashSet<Item>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Descricao { get; set; }

        [Required]
        public int Ordem { get; set; }

        [Required]
        public bool ExigeJustificativa { get; set; }

        [Required]
        public TipoQuestao Tipo { get; set; }

        [Required]
        public bool ExigeResposta { get; set; }

        [Required]
        [ForeignKey("Questionario")]
        public int IdQuestionario { get; set; }

        public virtual Questionario Questionario { get; set; }

        public virtual ICollection<Item> Itens { get; set; }

        public virtual int Peso
        {
            get
            {
                switch (Tipo)
                {
                    case TipoQuestao.Subjetiva:
                        return 0;
                    case TipoQuestao.ObjetivaUnicaEscolha:
                        return Itens.Max(i => i.Peso);
                    case TipoQuestao.ObjetivaMultiplaEscolha:
                        return Itens.Sum(i => i.Peso);
                    default:
                        return 0;
                      
                }
            }
        }
    }

    public enum TipoQuestao
    {
        Subjetiva,
        ObjetivaUnicaEscolha,
        ObjetivaMultiplaEscolha
    }
}