using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Data.Interfaces
{
    public interface IPerfilRepository : IRepository<IdentityRole>
    {
        IEnumerable<IdentityRole> GetExcetoAdministrador();

        IdentityRole GetGestor();
    }
}
