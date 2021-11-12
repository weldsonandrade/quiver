using Quiver.Core.Models;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        IEnumerable<Usuario> GetAtivosByEmpresa(int IdEmpresa);

        IEnumerable<Usuario> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string termo);

        IEnumerable<Usuario> GetByEmpresaAndEmail(int idEmpresa, string email);
    }
}
