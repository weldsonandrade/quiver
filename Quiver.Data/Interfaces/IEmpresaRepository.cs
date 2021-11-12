using Quiver.Core.Models;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        IEnumerable<Empresa> GetStartWithNome(string startWithNome);
    }
}
