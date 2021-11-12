using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class ConfiguracaoMapper
    {
        public static ConfiguracaoDTO MapConfiguracaoToConfiguracaoDTO(Configuracao configuracao)
        {
            return GetConfig().CreateMapper().Map<Configuracao, ConfiguracaoDTO>(configuracao);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Configuracao, ConfiguracaoDTO>();
            });
            return config;
        }
    }
}
