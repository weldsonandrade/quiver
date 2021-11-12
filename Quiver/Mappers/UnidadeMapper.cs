using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.DTO.Perfil;
using Quiver.DTO.Unidade;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



using System.Collections;

namespace Quiver.Mappers
{
    public class UnidadeMapper
    {

        public static UnidadePerfilVM MapUnidadeDTOToUnidadePerfilVM(UnidadeDTO unidadeDTO)
        {
            return GetConfig().CreateMapper().Map<UnidadeDTO, UnidadePerfilVM>(unidadeDTO);
        }

        private static MapperConfiguration GetConfig()
        {
            // Marca a cor de cada intervalo entre todas disponíveis.
            var config = new MapperConfiguration(cfg =>
            {
               // UsuarioDTO -> UsuarioPerfilVM
                cfg.CreateMap<UnidadeDTO, UnidadePerfilVM>()
                    .ForMember(u => u.avaliacoes, opt => opt.Ignore());
            });
            return config;
        }

    }
}