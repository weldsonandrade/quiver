using System;
using System.Collections.Generic;
using Quiver.Core.Models;
using Quiver.Data.Interfaces;

namespace Quiver.Data.Repository
{
    public class EmpresaRepository : GenericRepository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<Empresa> GetStartWithNome(string startWithNome)
        {
            return Get(filter: u => u.Nome.StartsWith(startWithNome));
        }
    }
}
