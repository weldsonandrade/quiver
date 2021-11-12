using AutoMapper;
using Quiver.Common.Utils;
using Quiver.DTO.Alternativa;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.AvaliacaoQuestionarioGrupo;
using Quiver.DTO.Questao;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quiver.Mappers
{
    public class AgendaMapper
    {
        private const String COR_NAO_INSPECIONADOS_E_ATRASADOS = "#D33";
        private const String COR_NAO_INSPECIONADOS = "#4285F4";
        private const String COR_INSPECIONADOS = "#1B5E20";

        private const String FORMATO_DATA_PARA_EVENTOS = "yyyy-MM-dd";

        public static AgendamentoVM MapAvaliacaoDTOToAgendamentoVM(AvaliacaoDTO avaliacao)
        {
            var avaliacaoVM = GetConfig().CreateMapper().Map<AvaliacaoDTO, AgendamentoVM>(avaliacao);

            foreach (var qAvaliacao in avaliacao.QuestionariosGrupo)
            {               
                // Marcando respostas
                foreach (var resposta in qAvaliacao.Respostas)
                {
                    foreach (var item in resposta.Itens)
                    {
                        // Achando a mesma questão da resposta.
                        var questao = avaliacaoVM.ListaFormulario.Single(a => a.Id == qAvaliacao.IdQuestionario).Questoes.Single(q => q.Id == resposta.IdQuestao);
                        questao.Resposta = resposta.Justificativa;
                        questao.Fotos = resposta.Fotos;
                        if (questao.Tipo != DTO.Enum.TipoQuestao.Subjetiva)
                        {
                            var alternativa = questao.Alternativas.FirstOrDefault(a => a.Id == item);
                            if (alternativa != null)
                            {
                                alternativa.Marcada = true;
                            }
                        }
                    }
                }
            }

            avaliacaoVM.PontuacaoEfetuada = avaliacaoVM.PontuacaoEfetuada ?? 0;
            avaliacaoVM.PontuacaoMaxima = avaliacaoVM.PontuacaoMaxima ?? 0;

            return avaliacaoVM;
        }

        public static AvaliacaoDTO MapAgendamentoVMToAvaliacaoDTO(AgendamentoVM avaliacao)
        {
            return GetConfig().CreateMapper().Map<AgendamentoVM, AvaliacaoDTO>(avaliacao);
        }

        public static List<object> MapEventoDTOToEvento(IList<EventoDTO> eventosDTO)
        {
            List<object> eventos = new List<object>();
            foreach (var evento in eventosDTO)
            {
                // Aplicação da cor conforme situação da inspeção.
                string cor = COR_INSPECIONADOS;
                if (evento.Situacao == DTO.Enum.SituacaoAvaliacao.NAO_AVALIADO)
                {
                    cor = evento.Data < DateTime.Today ? COR_NAO_INSPECIONADOS_E_ATRASADOS : COR_NAO_INSPECIONADOS;
                }

                eventos.Add(new
                {
                    id = evento.Id,
                    agendada = evento.Agendada,
                    conforme = evento.Conforme,
                    title = evento.Titulo,
                    start = evento.Data.ToString(FORMATO_DATA_PARA_EVENTOS),
                    end = evento.Data.ToString(FORMATO_DATA_PARA_EVENTOS),
                    color = cor
                });
            }
            return eventos;
        }

        public static IList<AvaliacaoVM> MapAvaliacaoDTOToAvaliacaoVM(IEnumerable<AvaliacaoDTO> avaliacoesDTO)
        {

            var avaliacoes = GetConfig().CreateMapper().Map<IEnumerable<AvaliacaoDTO>, IList<AvaliacaoVM>>(avaliacoesDTO);
            
            foreach (AvaliacaoVM avaliacao in avaliacoes)
            {                        
                if (avaliacao.Situacao == "NAO_AVALIADO")
                {
                    if (avaliacao.DataProgramada >= TZUtil.GetDataDeBrasilia())
                    {
                        avaliacao.Situacao = "ANDAMENTO";
                    }
                    else
                    {
                        avaliacao.Situacao = "ATRASADA";
                    }
                }
                else {
                    avaliacao.Situacao = "AVALIADO";
                }
            }
            return avaliacoes;
        }

        public static List<PontoEvolutivoGeralVM> MapAvaliacaoDTOToPontoEvolutivoGeralVM(IList<AvaliacaoDTO> avaliacoesDTO, string tipoGrafico, DateTime dataPrimeiraAvaliacao, DateTime dataUltimaAvaliacao)
        {
            var pontosNoGraficoDoUsuario = new List<PontoEvolutivoGeralVM>();
            for (DateTime dataAtual = dataPrimeiraAvaliacao; dataAtual <= dataUltimaAvaliacao; dataAtual = dataAtual.AddDays(1.0))
            {
                PontoEvolutivoGeralVM pontoDoGrafico;
                var avaliacoesDBdoDia = avaliacoesDTO.Where(a => a.DataFim.Value.Date == dataAtual.Date);

                if (avaliacoesDBdoDia.Count() > 0)
                {
                    pontoDoGrafico = new PontoEvolutivoGeralVM();
                    pontoDoGrafico.QtdAvaliacoes = avaliacoesDBdoDia.Count().ToString();
                    pontoDoGrafico.DataExecutada = dataAtual.ToString();
                    pontoDoGrafico.EfetividadeMedia = CalcularEfetividadeMedia(avaliacoesDBdoDia).ToString();
                    pontoDoGrafico.TimeStampDataExecutada = ToJavascriptTimestamp(dataAtual);
                    foreach (var avaliacaoDoDia in avaliacoesDBdoDia)
                    {
                        AvaliacaoPontoVM avaliacaoPontoVM = GetConfig().CreateMapper().Map<AvaliacaoDTO, AvaliacaoPontoVM>(avaliacaoDoDia);
                        pontoDoGrafico.avaliacoesDia.Add(avaliacaoPontoVM);
                    }
                    pontosNoGraficoDoUsuario.Add(pontoDoGrafico);
                }
                else if(avaliacoesDBdoDia.Count() == 0 && tipoGrafico != "efetividade")
                {
                    pontoDoGrafico = new PontoEvolutivoGeralVM();
                    pontoDoGrafico.QtdAvaliacoes = "0";
                    pontoDoGrafico.EfetividadeMedia = "0";
                    pontoDoGrafico.DataExecutada = dataAtual.ToString();
                    pontoDoGrafico.TimeStampDataExecutada = ToJavascriptTimestamp(dataAtual);
                    pontosNoGraficoDoUsuario.Add(pontoDoGrafico);
                }
               
            }
            return pontosNoGraficoDoUsuario;
        }


        public static List<PontoEvolutivoGeralVM> MapAvaliacaoDTOToPontoQuantidadeGeralVM(IList<AvaliacaoDTO> avaliacoesDTO, DateTime dataPrimeiraAvaliacao, DateTime dataUltimaAvaliacao)
        {
            var pontosNoGraficoDoUsuario = new List<PontoEvolutivoGeralVM>();
            for (DateTime dataAtual = dataPrimeiraAvaliacao; dataAtual <= dataUltimaAvaliacao; dataAtual = dataAtual.AddDays(1.0))
            {
                PontoEvolutivoGeralVM pontoDoGrafico;
                var avaliacoesDBdoDia = avaliacoesDTO.Where(a => a.DataFim.Value.Date == dataAtual.Date);
                if (avaliacoesDBdoDia.Count() > 0)
                {
                    pontoDoGrafico = new PontoEvolutivoGeralVM();
                    pontoDoGrafico.QtdAvaliacoes = avaliacoesDBdoDia.Count().ToString();
                    pontoDoGrafico.DataExecutada = dataAtual.ToString();
               
                    pontoDoGrafico.TimeStampDataExecutada = ToJavascriptTimestamp(dataAtual);

                    foreach (var avaliacaoDoDia in avaliacoesDBdoDia)
                    {
                        // Ver mapeamento
                        AvaliacaoPontoVM avaliacaoPontoVM = GetConfig().CreateMapper().Map<AvaliacaoDTO, AvaliacaoPontoVM>(avaliacaoDoDia);
                        pontoDoGrafico.avaliacoesDia.Add(avaliacaoPontoVM);
                    }

                    pontosNoGraficoDoUsuario.Add(pontoDoGrafico);
                }
                else
                {
                    pontoDoGrafico = new PontoEvolutivoGeralVM();
                    pontoDoGrafico.QtdAvaliacoes = "0";
                    pontoDoGrafico.DataExecutada = dataAtual.ToString();
                    pontoDoGrafico.TimeStampDataExecutada = ToJavascriptTimestamp(dataAtual);
                    pontosNoGraficoDoUsuario.Add(pontoDoGrafico);
                }
            }
            return pontosNoGraficoDoUsuario;
        }


        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // DTO -> AgendamentoVM
                cfg.CreateMap<AlternativaDTO, AlternativaRespondidaVM>();
                // A alternativa recebe o Id do item para comparação com o irem respondido.
                cfg.CreateMap<QuestaoDTO, QuestaoRespondidaVM>()
                    .ForMember(dest => dest.Alternativas, opt => opt.MapFrom(
                        src => src.Itens.Where(i => i.Alternativa != null).Select(i => new AlternativaDTO()
                        {
                            Id = i.Id,
                            Descricao = i.Alternativa.Descricao,
                            ExigeJustificativa = i.Alternativa.ExigeJustificativa,
                            NaoConformidade = i.Alternativa.NaoConformidade,
                            Ordem = i.Alternativa.Ordem,
                            Peso = i.Alternativa.Peso
                        })));
                cfg.CreateMap<AvaliacaoQuestionarioGrupoDTO, QuestionarioRespondidoVM>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdQuestionario))
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.QuestionarioGrupo.Questionario.Nome))
                    .ForMember(dest => dest.Questoes, opt => opt.MapFrom(src => src.QuestionarioGrupo.Questionario.Questoes));
                cfg.CreateMap<AvaliacaoDTO, AgendamentoVM>()
                    .ForMember(dest => dest.IdGrupo, opt => opt.MapFrom(src => src.QuestionariosGrupo.First().QuestionarioGrupo.IdGrupo))
                    .ForMember(dest => dest.NomeGrupo, opt => opt.MapFrom(src => src.QuestionariosGrupo.First().QuestionarioGrupo.Grupo.Nome))
                    .ForMember(dest => dest.DataExecutada, opt => opt.MapFrom(src => src.DataFim))
                    .ForMember(dest => dest.NomeUnidade, opt => opt.MapFrom(src => src.Unidade.Nome))
                    .ForMember(dest => dest.ListaFormulario, opt => opt.MapFrom(src => src.QuestionariosGrupo))
                    .ForMember(dest => dest.LocalizacaoLatitude, opt => opt.MapFrom(src => src.Latitude))
                    .ForMember(dest => dest.LocalizacaoLongitude, opt => opt.MapFrom(src => src.Longitude));
                // VM -> DTO
                cfg.CreateMap<AgendamentoVM, AvaliacaoDTO>();

                // AvaliacaoDTO -> AvaliacaoVM
                cfg.CreateMap<AvaliacaoDTO, AvaliacaoVM>()
                    .ForMember(a => a.DataExecutada, opt => opt.MapFrom(src => src.DataFim))
                    .ForMember(a => a.NomeUsuario, opt => opt.MapFrom(src => src.NomeUsuario))
                    .ForMember(a => a.EmailUsuario, opt => opt.MapFrom(src => src.EmailUsuario))
                    .ForMember(a => a.Conforme, opt => opt.MapFrom(src => src.Conforme))
                    .ForMember(a => a.NomeGrupo, opt => opt.MapFrom(src => src.QuestionariosGrupo.FirstOrDefault().QuestionarioGrupo.Grupo.Nome))
                    .ForMember(a => a.NomeUnidade, opt => opt.MapFrom(src => src.Unidade.Nome));
                // AvaliacaoDTO -> AvaliacaoPontoVM
                cfg.CreateMap<AvaliacaoDTO, AvaliacaoPontoVM>()
                    .ForMember(a => a.nomeUnidade, opt => opt.MapFrom(src => src.Unidade.Nome))
                    .ForMember(a => a.rotulo, opt => opt.MapFrom(src => src.RotuloCalendario))
                    .ForMember(a => a.TimeStampDataExecutada, opt => opt.MapFrom(src => ToJavascriptTimestamp(src.DataFim ?? src.DataProgramada)))
                    .ForMember(a => a.DataExecutada, opt => opt.MapFrom(src => src.DataFim.ToString()))
                    .ForMember(a => a.efetividade, opt => opt.MapFrom(src => src.PontuacaoMaxima == 0 ? 0 : (100 * src.PontuacaoEfetuada) / src.PontuacaoMaxima));
            });
            return config;
        }

        private static long ToJavascriptTimestamp(DateTime input)
        {
            TimeSpan span = new TimeSpan(new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            DateTime time = input.Subtract(span);
            return (long)(time.Ticks / 10000L);
        }

        private static double CalcularEfetividadeMedia(IEnumerable<AvaliacaoDTO> avaliacoes)
        {
            avaliacoes = avaliacoes.Where(a => a.PontuacaoMaxima > 0);
            double somatorioEfetividades = 0;
            foreach (var avaliacao in avaliacoes)
            {
                somatorioEfetividades = somatorioEfetividades + calcularEfetividade(avaliacao);
            }
            return somatorioEfetividades / avaliacoes.Count();
        }

        private static double calcularEfetividade(AvaliacaoDTO a)
        {
                var total = a.PontuacaoMaxima;
                if (total > 0)
                {
                    var resultado = (100 * a.PontuacaoEfetuada) / total;
                    return resultado;
                }
                return 0;
        }


    }
}