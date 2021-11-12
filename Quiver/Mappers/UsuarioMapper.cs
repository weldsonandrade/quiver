using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Quiver.DTO.Perfil;
using Quiver.DTO.Usuario;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mappers
{
    public class UsuarioMapper
    {
        public static IList<UsuarioRow> MapUsuarioDTOToUsuarioRow(IList<UsuarioDTO> usuariosDTO, string idUsuarioLogado)
        {
            IList<UsuarioRow> usuarios = GetConfig().CreateMapper().Map<IList<UsuarioDTO>, IList<UsuarioRow>>(usuariosDTO);
            usuarios.Where(u => u.Id == idUsuarioLogado).SingleOrDefault().Logado = true;
            return usuarios;
        }

        public static UsuarioPerfilVM MapUsuarioDTOToUsuarioPerfilVM(UsuarioDTO usuarioDTO)
        {
            return GetConfig().CreateMapper().Map<UsuarioDTO, UsuarioPerfilVM>(usuarioDTO);
        }

        public static CriarUsuarioDTO MapUsuarioVMToCriarUsuarioDTO(UsuarioVM usuario, string perfilUsuarioLogado, int idEmpresaLogada)
        {
            CriarUsuarioDTO criarUsuarioDTO = GetConfig().CreateMapper().Map<UsuarioVM, CriarUsuarioDTO>(usuario);
            criarUsuarioDTO.PerfilUsuarioLogado = perfilUsuarioLogado;
            criarUsuarioDTO.IdEmpresaLogada = idEmpresaLogada;
            return criarUsuarioDTO;
        }

        private static MapperConfiguration GetConfig()
        {
            // Marca a cor de cada intervalo entre todas disponíveis.
            var config = new MapperConfiguration(cfg =>
            {
                // DTO -> VM
                cfg.CreateMap<UsuarioDTO, UsuarioRow>()
                    .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName));
                // UsuarioDTO -> UsuarioPerfilVM
                cfg.CreateMap<UsuarioDTO, UsuarioPerfilVM>()
                    .ForMember(u => u.Login, opt => opt.MapFrom(src => src.Email))
                    .ForMember(u => u.avaliacoes, opt => opt.Ignore());
                // UsuarioVM -> CriarUsuarioDTO
                cfg.CreateMap<UsuarioVM, CriarUsuarioDTO>()
                    .ForMember(u => u.Email, opt => opt.MapFrom(src => src.Login));
            });
            return config;
        }
    }
}