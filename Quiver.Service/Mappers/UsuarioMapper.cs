using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.Core.Models;
using Quiver.DTO.Perfil;
using Quiver.DTO.Usuario;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quiver.Service.Mappers
{
    public class UsuarioMapper
    {
        public static UsuarioDTO MapUsuarioToUsuarioDTO(Usuario usuario)
        {
            return GetConfig().CreateMapper().Map<Usuario, UsuarioDTO>(usuario);
        }

        public static IList<UsuarioDTO> MapUsuarioToUsuarioDTO(IEnumerable<Usuario> usuario)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Usuario>, IList<UsuarioDTO>>(usuario);
        }

        public static PerfilDTO MapIdentityRoleToPerfilDTO(IdentityRole perfil)
        {
            return GetConfig().CreateMapper().Map<IdentityRole, PerfilDTO>(perfil);
        }

        public static IList<PerfilDTO> MapIdentityRoleToPerfilDTO(IEnumerable<IdentityRole> perfis)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<IdentityRole>, IList<PerfilDTO>>(perfis);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Usuario, UsuarioDTO>()
                    .ForMember(dest => dest.IdPerfil, opt => opt.MapFrom(src => src.Roles.First().RoleId));
                // IdentityRole -> PerfilDTO
                cfg.CreateMap<IdentityRole, PerfilDTO>()
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Name));
            });
            return config;
        }
    }
}
