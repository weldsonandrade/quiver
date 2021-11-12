using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.Alternativa;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.AvaliacaoQuestionarioGrupo;
using Quiver.DTO.Grupo;
using Quiver.DTO.Item;
using Quiver.DTO.Questao;
using Quiver.DTO.Questionario;
using Quiver.DTO.QuestionarioGrupo;
using Quiver.DTO.Resposta;
using Quiver.DTO.Unidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class AgendaMapper
    {
        public static AvaliacaoDTO MapAvaliacaoToAvaliacaoDTO(Avaliacao avaliacao)
        {
            return GetConfig().CreateMapper().Map<Avaliacao, AvaliacaoDTO>(avaliacao);
        }

        public static IList<AvaliacaoDTO> MapAvaliacaoToAvaliacaoDTO(IEnumerable<Avaliacao> avaliacoes)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Avaliacao>, IList<AvaliacaoDTO>>(avaliacoes);
        }

        public static IList<EventoDTO> MapAvaliacaoToEventoDTO(IEnumerable<Avaliacao> avaliacoes)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Avaliacao>, IList<EventoDTO>>(avaliacoes);
        }

        public static Avaliacao MapAvaliacaoDTOToAvaliacao(AvaliacaoDTO avaliacao)
        {
            return GetConfig().CreateMapper().Map<AvaliacaoDTO, Avaliacao>(avaliacao);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Avaliacao, AvaliacaoDTO>()
                    .ForMember(dest => dest.QuestionariosGrupo, opt => opt.MapFrom(src => src.QuestionariosAvaliacao))
                    .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.Nome))
                    .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.LocalizacaoLatitude))
                    .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.LocalizacaoLongitude))
                    .ForMember(dest => dest.IdEmpresa, opt => opt.MapFrom(src => src.Usuario.IdEmpresa));
                cfg.CreateMap<AvaliacaoQuestionarioGrupo, AvaliacaoQuestionarioGrupoDTO>();
                cfg.CreateMap<QuestionarioGrupo, QuestionarioGrupoDTO>()
                    .ForMember(dest => dest.Avaliacoes, opt => opt.Ignore());
                cfg.CreateMap<Unidade, UnidadeDTO>();
                cfg.CreateMap<Questionario, QuestionarioDTO>()
                    .ForMember(dest => dest.Grupos, opt => opt.Ignore());
                cfg.CreateMap<Resposta, RespostaDTO>(); 
                cfg.CreateMap<Questao, QuestaoDTO>();
                cfg.CreateMap<Item, ItemDTO>();
                cfg.CreateMap<Alternativa, AlternativaDTO>();
                cfg.CreateMap<Grupo, GrupoDTO>()
                    .ForMember(dest => dest.Classificacoes, opt => opt.Ignore())
                    .ForMember(dest => dest.Questionarios, opt => opt.Ignore());
                cfg.CreateMap<Resposta, RespostaDTO>()
                    .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens.Select(ri => ri.Item.Id).ToList()))
                    .ForMember(dest => dest.Fotos, opt => opt.MapFrom(src => src.Fotos.Select(f => f.Fotografia).ToList()))
                    .ForMember(dest => dest.IdQuestao, opt => opt.MapFrom(src => src.Itens.FirstOrDefault().Item.IdQuestao));
                cfg.CreateMap<AvaliacaoDTO, Avaliacao>();
                cfg.CreateMap<UnidadeDTO, Unidade>();
                cfg.CreateMap<Avaliacao, EventoDTO>()
                    .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.RotuloCalendario))
                    .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DataProgramada));
            });
            return config;
        }
    }
}
