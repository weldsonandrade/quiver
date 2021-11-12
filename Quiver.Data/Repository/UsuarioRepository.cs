using System;
using System.Collections.Generic;
using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Quiver.Data.Repository
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<Usuario> GetAtivosByEmpresa(int idEmpresa)
        {
            return Get(u => u.IdEmpresa == idEmpresa && u.LockoutEnabled == false).ToList();
        }

        public IEnumerable<Usuario> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo)
        {
            return Get(
                    filter: u => u.UserName.StartsWith(termo)
                    && u.IdEmpresa == idEmpresa
                    && u.LockoutEnabled == false
                ).OrderBy(u => u.UserName);
        }

        public IEnumerable<Usuario> GetByEmpresaAndEmail(int idEmpresa, string email)
        {
            return Get(filter: u => u.IdEmpresa == idEmpresa && u.Email == email);
        }
    }
}
