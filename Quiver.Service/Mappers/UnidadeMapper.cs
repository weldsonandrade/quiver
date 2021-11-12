using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.Core.Models;
using Quiver.DTO.Perfil;
using Quiver.DTO.Unidade;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quiver.Service.Mappers
{
    public class UnidadeMapper
    {
        public static UnidadeDTO MapUnidadeToUnidadeDTO(Unidade unidade)
        {
            return GetConfig().CreateMapper().Map<Unidade, UnidadeDTO>(unidade);
        }

        public static IList<UnidadeDTO> MapUnidadeToUnidadeDTO(IEnumerable<Unidade> unidade)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Unidade>, IList<UnidadeDTO>>(unidade);
        }


        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Unidade, UnidadeDTO>();
            });
            return config;
        }
    }
}
