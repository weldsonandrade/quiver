using AutoMapper;
using Quiver.Common.Utils;
using Quiver.DTO.PlanoAcao;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mappers
{
    public class PlanoAcaoMapper
    {
        public static IList<PlanoAcaoRowVM> MapPlanoAcaoDTOToPlanoAcaoVM(IList<PlanoAcaoDTO> planosAcao)
        {
            return GetConfig().CreateMapper().Map<IList<PlanoAcaoDTO>, IList<PlanoAcaoRowVM>>(planosAcao);
        }

        public static PlanoAcaoCardVM MapPlanoAcaoDTOToPlanoAcaoCardVM(IList<PlanoAcaoDTO> planosAcao)
        {
            IList<PlanoAcaoRowVM> planosAcaoVM = MapPlanoAcaoDTOToPlanoAcaoVM(planosAcao);
            List<PlanoAcaoAEditarRowVM> planoAcaoAEditarRowVMList = new List<PlanoAcaoAEditarRowVM>();
            var planosAeditar = planosAcao.Where(p => p.Situacao == DTO.Enum.SituacaoPlanoAcao.A_EDITAR).ToList();
            foreach (var planoAcao in planosAeditar)
            {
                planoAcaoAEditarRowVMList.Add(new PlanoAcaoAEditarRowVM()
                {
                    Id = planoAcao.Id,
                    Data = planoAcao.Questao.Formulario.Avaliacao.DataFim,
                    Avaliacao = planoAcao.Questao.Formulario.Avaliacao.Rotulo,
                    Item = planoAcao.Questao.Descricao,
                    Alternativa = planoAcao.Questao.Alternativas.Single(a => a.Respondida == true).Descricao
                });
            }
            
            PlanoAcaoCardVM planoAcaoCardVM = new PlanoAcaoCardVM()
            {
                PlanosAcaoAEditar = planoAcaoAEditarRowVMList,
                //PlanosAcaoAtrasados = planosAcaoVM.Where(p => p.Situacao == Core.Models.SituacaoPlanoAcao.ANDAMENTO && p.Quando < TZUtil.GetDataDeBrasilia()).ToList(),
                PlanosAcaoAndamento = planosAcaoVM.Where(p => p.Situacao == Core.Models.SituacaoPlanoAcao.ANDAMENTO).ToList(),
                PlanosAcaoCancelados = planosAcaoVM.Where(p => p.Situacao == Core.Models.SituacaoPlanoAcao.CANCELADO).ToList(),
                PlanosAcaoEncerrados = planosAcaoVM.Where(p => p.Situacao == Core.Models.SituacaoPlanoAcao.ENCERRADO).ToList(),
                //PlanosAcaoResolvidos = planosAcaoVM.Where(p => p.Situacao == Core.Models.SituacaoPlanoAcao.RESOLVIDO).ToList()
            };
            return planoAcaoCardVM;
        }

        public static PlanoAcaoVM MapPlanoAcaoDTOToPlanoAcaoVM(PlanoAcaoDTO planoAcaoDTO)
        {
            return GetConfig().CreateMapper().Map<PlanoAcaoDTO, PlanoAcaoVM>(planoAcaoDTO);
        }

        public static PlanoAcaoDTO MapPlanoAcaoVMTOPlanoAcaoDTO(PlanoAcaoVM planoAcaoVM)
        {
            return GetConfig().CreateMapper().Map<PlanoAcaoVM, PlanoAcaoDTO>(planoAcaoVM);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // DTO -> PlanoAcaoRowVM
                cfg.CreateMap<PlanoAcaoDTO, PlanoAcaoRowVM>();
                // DTO -> PlanoAcaoVM
                cfg.CreateMap<PlanoAcaoDTO, PlanoAcaoVM>()
                    .ForMember(dest => dest.Origem, opt => opt.MapFrom(src => src.Questao))
                    .ForMember(dest => dest.Observaocao, opt => opt.MapFrom(src => src.Justificativa));
                cfg.CreateMap<QuestaoDTO, OrigemNaoConformidadeVM>()
                    .ForMember(dest => dest.Rotulo, opt => opt.MapFrom(src => src.Formulario.Avaliacao.Rotulo))
                    .ForMember(dest => dest.Formulario, opt => opt.MapFrom(src => src.Formulario.Descricao))
                    .ForMember(dest => dest.Grupo, opt => opt.MapFrom(src => src.Formulario.Grupo))
                    .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Descricao))
                    .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Formulario.Avaliacao.Usuario))
                    .ForMember(dest => dest.DataExecucao, opt => opt.MapFrom(src => src.Formulario.Avaliacao.DataFim))
                    .ForMember(dest => dest.Unidade, opt => opt.MapFrom(src => src.Formulario.Avaliacao.Unidade))
                    .ForMember(dest => dest.Alternativas, opt => opt.MapFrom(src => src.Alternativas));
                cfg.CreateMap<AlternativaDTO, AlternativaRespondidaVM>()
                    .ForMember(dest => dest.Marcada, opt => opt.MapFrom(src => src.Respondida));
                // PlanoAcaoVM -> DTO
                cfg.CreateMap<PlanoAcaoVM, PlanoAcaoDTO>()
                    .ForMember(dest => dest.Questao, opt => opt.MapFrom(src => src.Origem))
                    .ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => src.Observaocao));
                cfg.CreateMap<OrigemNaoConformidadeVM, QuestaoDTO>()
                    .ForMember(dest => dest.Formulario, opt => opt.MapFrom(f => new FormularioDTO() {
                        Descricao = f.Formulario, 
                        Grupo = f.Grupo,
                        Avaliacao = new AvaliacaoDTO() {
                                Rotulo = f.Rotulo, Usuario = f.Usuario, Unidade = f.Unidade
                            }
                        }))
                    .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Item))
                    .ForMember(dest => dest.Alternativas, opt => opt.MapFrom(src => src.Alternativas));
                cfg.CreateMap<AlternativaRespondidaVM, AlternativaDTO>()
                    .ForMember(dest => dest.Respondida, opt => opt.MapFrom(src => src.Marcada));
            });
            return config;
        }
    }
}