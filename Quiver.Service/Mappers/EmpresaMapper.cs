using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class EmpresaMapper
    {
        public static EmpresaDTO MapEmpresaToEmpresaDTO(Empresa empresa)
        {
            return GetConfig().CreateMapper().Map<Empresa, EmpresaDTO>(empresa);
        }

        public static IList<EmpresaDTO> MapEmpresaToEmpresaDTO(IEnumerable<Empresa> empresa)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Empresa>, IList<EmpresaDTO>>(empresa);
        }

        public static Empresa MapEmpresaDTOToEmpresa(EmpresaDTO empresaDTO)
        {
            return GetConfig().CreateMapper().Map<EmpresaDTO, Empresa>(empresaDTO);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Empresa, EmpresaDTO>();
                cfg.CreateMap<EmpresaDTO, Empresa>();
            });
            return config;
        }
    }
}
