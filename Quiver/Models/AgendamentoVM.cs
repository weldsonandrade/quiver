using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class AgendamentoVM
    {
        public int? Id { get; set; }

        public string RotuloCalendario { get; set; }

        public int IdUnidade { get; set; }

        public string NomeUnidade { get; set; }

        public int IdGrupo { get; set; }

        public string NomeGrupo { get; set; }

        public string IdUsuario { get; set; }

        public string NomeUsuario { get; set; }

        public string EmailUsuario { get; set; }

        public DateTime DataProgramada { get; set; }

        public DateTime? DataExecutada { get; set; }

        public string ObservacaoAvaliacao { get; set; }

        public double LocalizacaoLatitude { get; set; }

        public double LocalizacaoLongitude { get; set; }

        public string Dispositivo { get; set; }

        public int? PontuacaoMaxima { get; set; }

        public int? PontuacaoEfetuada { get; set; }

        public string Assinatura { get; set; }

        public string NomeResponsavel { get; set; }

        public string CargoResponsavel { get; set; }

        public List<QuestionarioRespondidoVM> ListaFormulario { get; set; }

        public AgendamentoVM()
        {
            this.ListaFormulario = new List<QuestionarioRespondidoVM>();
        }

        public AgendamentoVM(int Id, DateTime dataProgramada)
        {
            this.Id = Id;
            this.DataProgramada = dataProgramada;
        }

        #region Recorrencia
        public bool PossuiRecorrencia { get; set; }

        public RecorrenciaVM Recorrencia { get; set; }

        public TipoManipulacao Tipo { get; set; }

        public class RecorrenciaVM
        {
            #region Geral

            public FrequenciaRecorrencia? Frequencia { get; set; }

            public DateTime? DataInicial { get; set; }

            public DateTime? DataFinal { get; set; }

            public int? QuantidadeDeRepeticoes { get; set; }

            #region Diaria

            public double? IntervaloDias { get; set; }

            #endregion

            #region Semanal

            public int DiasDaSemana { get; set; }

            #endregion

            #region Mensal

            public List<int> DiasDoMes { get; set; }

            #endregion

            #endregion

            #region Customizada

            public List<DateTime> DatasCustomizadas { get; set; }

            #endregion
        }

        #endregion
    }

    // VM do item de formulario respondido
    public class QuestionarioRespondidoVM
    {
        public QuestionarioRespondidoVM()
        {
            this.Questoes = new List<QuestaoRespondidaVM>();
        }

        public int Id { get; set; }


        public string Nome { get; set; }

        public int Responsavel { get; set; }

        public int Grupo { get; set; }

        public List<QuestaoRespondidaVM> Questoes { get; set; }

    }

    // VM do item de formulario respondido
    public class QuestaoRespondidaVM
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public string Resposta { get; set; }

        public DTO.Enum.TipoQuestao Tipo { get; set; }

        public List<AlternativaRespondidaVM> Alternativas { get; set; }

        public List<String> Fotos { get; set; }

        public QuestaoRespondidaVM()
        {
            this.Alternativas = new List<AlternativaRespondidaVM>();
            this.Fotos = new List<String>();
        }
    }

    // VM das alternativas
    public class AlternativaRespondidaVM
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public Boolean Marcada { get; set; }

        public bool NaoConformidade { get; set; }

        public int Peso { get; set; }
    }
}