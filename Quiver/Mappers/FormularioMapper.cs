using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.Alternativa;
using Quiver.DTO.Item;
using Quiver.DTO.Questao;
using Quiver.DTO.Questionario;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mappers
{
    public class FormularioMapper
    {
        public static QuestionarioDTO MapQuestionarioVMToQuestionarioDTO(QuestionarioVM questionario)
        {
            return GetConfig().CreateMapper().Map<QuestionarioVM, QuestionarioDTO>(questionario);
        }

        public static QuestionarioVM MapQuestionarioDTOToQuestionarioVM(QuestionarioDTO questionario)
        {
            return GetConfig().CreateMapper().Map<QuestionarioDTO, QuestionarioVM>(questionario);
        }

        public static List<QuestionarioRowVM> MapQuestionarioDTOToQuestionarioRowVM(IEnumerable<QuestionarioDTO> questionarios)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<QuestionarioDTO>, List<QuestionarioRowVM>>(questionarios);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // DTO -> VM
                cfg.CreateMap<QuestionarioDTO, QuestionarioRowVM>()
                    .ForMember(dest => dest.Grupos, opt => opt.MapFrom(src => String.Join(", ", src.Grupos.Select(g => g.Grupo.Nome))));
                cfg.CreateMap<AlternativaDTO, AlternativaVM>();
                // Converte item para Alternativas.
                cfg.CreateMap<QuestaoDTO, QuestaoVM>()
                    .ForMember(dest => dest.Alternativas, opt => opt.MapFrom(src => src.Itens.Where(i => i.Alternativa != null).Select(i => new AlternativaVM()
                    {
                        Id = i.Alternativa.Id,
                        Descricao = i.Alternativa.Descricao,
                        ExigeJustificativa = i.Alternativa.ExigeJustificativa,
                        NaoConformidade = i.Alternativa.NaoConformidade,
                        Ordem = i.Alternativa.Ordem,
                        Peso = i.Alternativa.Peso
                    })));
                cfg.CreateMap<QuestionarioDTO, QuestionarioVM>();

                // VM -> DTO
                cfg.CreateMap<QuestionarioVM, QuestionarioDTO>()
                    .ForMember(dest => dest.Grupos, opt => opt.Ignore());
                cfg.CreateMap<QuestaoVM, QuestaoDTO>()
                    .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Alternativas.Select(a => new ItemDTO()
                    {
                        Alternativa = new AlternativaDTO()
                        {
                            Id = a.Id,
                            Descricao = a.Descricao,
                            ExigeJustificativa = a.ExigeJustificativa,
                            NaoConformidade = a.NaoConformidade,
                            Ordem = a.Ordem,
                            Peso = a.Peso
                        }
                    })));
                cfg.CreateMap<AlternativaVM, AlternativaDTO>();
            });
            return config;
        }
    }
}