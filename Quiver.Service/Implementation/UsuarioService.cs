using Quiver.Data.Interfaces;
using Quiver.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiver.DTO.Usuario;
using Quiver.Service.Mappers;
using Quiver.DTO.Perfil;
using Quiver.Core.Models;
using Quiver.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Quiver.Infrastructure.Configuration;
using Quiver.Common.Utils;

namespace Quiver.Service.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private IUnitOfWork _uow;

        public UsuarioService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void Delete(string idUsuario)
        {
            var usuarioToDelete = _uow.UsuarioRepository.GetByID(idUsuario);
            usuarioToDelete.UserName = Guid.NewGuid().ToString();
            usuarioToDelete.LockoutEnabled = true;
            _uow.UsuarioRepository.Update(usuarioToDelete);
            _uow.SaveChanges();
        }

        public IList<UsuarioDTO> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo)
        {
            var usuarios = _uow.UsuarioRepository.GetAtivosByEmpresaAndStartWithNome(idEmpresa, termo);
            IList<PerfilDTO> perfis = GetPerfis();
           
            IList<UsuarioDTO> usuariosDTO = UsuarioMapper.MapUsuarioToUsuarioDTO(usuarios);

            foreach (var usuarioDTO in usuariosDTO)
            {
                usuarioDTO.Perfil = perfis.Single(p => p.Id == usuarioDTO.IdPerfil).Nome; 
            }

            return usuariosDTO;
        }

        public IList<UsuarioDTO> GetAtivosByEmpresa(int IdEmpresa)
        {
            var inspetores = _uow.UsuarioRepository.GetAtivosByEmpresa(IdEmpresa);
            return UsuarioMapper.MapUsuarioToUsuarioDTO(inspetores);
        }

        public UsuarioDTO GetById(string idUsuario)
        {
            var usuario = _uow.UsuarioRepository.GetByID(idUsuario);
            return UsuarioMapper.MapUsuarioToUsuarioDTO(usuario);
        }

        public PerfilDTO GetPerfilById(string idPerfil)
        {
            var perfil = _uow.RoleRepository.GetByID(idPerfil);
            return UsuarioMapper.MapIdentityRoleToPerfilDTO(perfil);
        }

        public PerfilDTO GetPerfilGestor()
        {
            var perfil = _uow.RoleRepository.GetGestor();
            return UsuarioMapper.MapIdentityRoleToPerfilDTO(perfil);
        }

        public IList<PerfilDTO> GetPerfis()
        {
            var perfis = _uow.RoleRepository.Get();
            return UsuarioMapper.MapIdentityRoleToPerfilDTO(perfis);
        }

        public IList<PerfilDTO> GetPerfisExcetoAdministrador()
        {
            var perfis = _uow.RoleRepository.GetExcetoAdministrador();
            return UsuarioMapper.MapIdentityRoleToPerfilDTO(perfis);
        }

        public IdentityResult Insert(CriarUsuarioDTO usuario)
        {
            Empresa empresa = null;
            if (usuario.PerfilUsuarioLogado == "Administrador")
                empresa = _uow.EmpresaRepository.GetByID(Convert.ToInt32(usuario.IdEmpresa));
            else
                empresa = _uow.EmpresaRepository.GetByID(usuario.IdEmpresaLogada);

            var gestorRoleId = GetPerfilGestor().Id;
            var licencasEmUso = _uow.UsuarioRepository.GetAtivosByEmpresa(empresa.Id).Count();

            if (licencasEmUso >= empresa.LimiteLicencas)
                throw new Exception("Você atingiu seu limite de licenças!");

            var nomePerfil = GetPerfilById(usuario.Perfil).Nome;

            usuario.Email = usuario.Email.ToLower();

            List<Usuario> usuarios = _uow.UsuarioRepository.GetByEmpresaAndEmail(empresa.Id, usuario.Email).ToList();
            Usuario usuarioExistente = null;
            foreach (Usuario u in usuarios)
            {
                string uNomePerfil = GetPerfilById(u.Roles.FirstOrDefault().RoleId).Nome;
                if (uNomePerfil == nomePerfil && u.Nome == usuario.Nome && u.LockoutEnabled == true && u.Email != u.UserName)
                {
                    usuarioExistente = u;
                }
            }

            // Se não tiver senha o sistema gera uma.
            if (string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = RandomPassword.Generate(8);
            }

            var store = new UserStore<Usuario>(_uow.GetDbContext<QuiverDbContext>());
            var userManager = new ApplicationUserManager(store, null);            

            // Se não existe, então cria.
            if (usuarioExistente == null)
            {
                var usuarioToInsert = new Usuario()
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Empresa = empresa,
                    EmailConfirmed = false,
                    PhoneNumber = usuario.Telefone,
                    PhoneNumberConfirmed = false
                };

                // Só existe um usuario admin no sistema
                if (nomePerfil == "Administrador")
                {
                    throw new Exception("Perfil inválido.");
                }

                IdentityResult createResult = userManager.Create(usuarioToInsert, usuario.Senha);

                if (createResult.Succeeded)
                {                                        
                    userManager.AddToRole(usuarioToInsert.Id, nomePerfil);
                    usuario.Id = usuarioToInsert.Id;
                } else
                {
                    usuario.Id = userManager.FindByEmail(usuario.Email).Id;
                }
                return createResult;
            }
            else
            {
                usuario.Id = usuarioExistente.Id;

                usuarioExistente.UserName = usuario.Email;
                usuarioExistente.LockoutEnabled = false;
                usuarioExistente.PasswordHash = userManager.PasswordHasher.HashPassword(usuario.Senha);
                _uow.UsuarioRepository.Update(usuarioExistente);
                _uow.SaveChanges();                
            }
            return null;
        }
    }
}
