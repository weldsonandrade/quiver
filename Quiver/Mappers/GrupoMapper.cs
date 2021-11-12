using AutoMapper;
using Quiver.DTO.Classificacao;
using Quiver.DTO.Grupo;
using Quiver.DTO.QuestionarioGrupo;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiver.Mappers
{
    public class GrupoMapper
    {
        public static GrupoVM MapGrupoDTOToGrupoVM(GrupoDTO grupo)
        {
            return GetConfig().CreateMapper().Map<GrupoDTO, GrupoVM>(grupo);
        }


        public static GrupoVM MapGrupoFormularioDTOToGrupoVM(GrupoDTO grupo)
        {
            return GetConfig().CreateMapper().Map<GrupoDTO, GrupoVM>(grupo);
        }



        
        public static GrupoDTO MapGrupoVMToGrupoDTO(GrupoVM grupo)
        {
            return GetConfig().CreateMapper().Map<GrupoVM, GrupoDTO>(grupo);
        }

        public static IList<ClassificacaoViewModels> MapClassificacaoDTOTOClassificacaoViewModels(IList<ClassificacaoDTO> classificacoes)
        {
            return GetConfig().CreateMapper().Map<IList<ClassificacaoDTO>, IList<ClassificacaoViewModels>>(classificacoes);
        }

        public static GrupoDTO MapGrupoFormulariosVMToGrupoDTO(GrupoFormulariosVM grupoFormulariosVM)
        {
            return GetConfig().CreateMapper().Map<GrupoFormulariosVM, GrupoDTO>(grupoFormulariosVM);
        }

        private static MapperConfiguration GetConfig()
        {
            // Marca a cor de cada intervalo entre todas disponíveis.
            var config = new MapperConfiguration(cfg =>
            {
                // DTO -> VM
                cfg.CreateMap<GrupoDTO, GrupoVM>()
                    .ForMember(dest => dest.ListaClassificacoes, opt => opt.MapFrom(src => src.Classificacoes));
                cfg.CreateMap<ClassificacaoDTO, ClassificacaoViewModels>()
                    .ForMember(dest => dest.CorIntervaloClassificacao, opt => opt.MapFrom(src => src.Cor))
                    .ForMember(dest => dest.ListaCoresIntervalo, opt => opt.MapFrom(src => GrupoVM.CorClassificacao.Select(c => new SelectListItem()
                    {
                        Value = c,
                        Selected = c == src.Cor
                    })));
                // VM -> DTO
                cfg.CreateMap<GrupoVM, GrupoDTO>()
                    .ForMember(dest => dest.Classificacoes, opt => opt.MapFrom(src => src.ListaClassificacoes));
                cfg.CreateMap<ClassificacaoViewModels, ClassificacaoDTO>()
                    .ForMember(dest => dest.Cor, opt => opt.MapFrom(src => src.CorIntervaloClassificacao));
                // ClassificacaoDTO -> ClassificacaoViewModels
                cfg.CreateMap<ClassificacaoDTO, ClassificacaoViewModels>()
                   .ForMember(dest => dest.CorIntervaloClassificacao, opt => opt.MapFrom(src => src.Cor));
                // GrupoFormulariosDTO -> GrupoDTO
                cfg.CreateMap<GrupoFormulariosVM, GrupoDTO>()
                    .ForMember(dest => dest.Questionarios, opt => opt.MapFrom(src => src.Formularios));
                cfg.CreateMap<FormularioSelectedVM, QuestionarioGrupoDTO>()
                    .ForMember(dest => dest.IdQuestionario, opt => opt.MapFrom(src => src.Id));
            });
            return config;
        }
    }
}