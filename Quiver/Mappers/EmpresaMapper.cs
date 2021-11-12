using AutoMapper;
using Quiver.DTO.Empresa;
using Quiver.DTO.Enum;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mappers
{
    public class EmpresaMapper
    {
        public static EmpresaDTO MapEmpresaEditorVMToEmpresaDTO(EmpresaEditorVM empresa)
        {
            return GetConfig().CreateMapper().Map<EmpresaEditorVM, EmpresaDTO>(empresa);
        }

        public static EmpresaEditorVM MapEmpresaDTOToEmpresaEditorVM(EmpresaDTO empresa)
        {
            return GetConfig().CreateMapper().Map<EmpresaDTO, EmpresaEditorVM>(empresa);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // VM -> DTO
                cfg.CreateMap<EmpresaEditorVM, EmpresaDTO>()
                    .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao ? SituacaoEmpresa.ATIVA : SituacaoEmpresa.DESATIVA));
                // DTO -> EditorVM
                cfg.CreateMap<EmpresaDTO, EmpresaEditorVM>()
                    .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao == SituacaoEmpresa.ATIVA ? true : false));
            });
            return config;
        }
    }
}