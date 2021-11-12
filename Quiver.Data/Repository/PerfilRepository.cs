using Quiver.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Quiver.Data.Repository
{
    public class PerfilRepository : GenericRepository<IdentityRole>, IPerfilRepository
    {
        public PerfilRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<IdentityRole> GetExcetoAdministrador()
        {
            return Get(r => r.Name != "Administrador");
        }

        public IdentityRole GetGestor()
        {
            return Get(r => r.Name == "Gestor").FirstOrDefault();
        }
    }
}
