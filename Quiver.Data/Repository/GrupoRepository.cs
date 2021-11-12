using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

namespace Quiver.Data.Repository
{
    public class GrupoRepository : GenericRepository<Grupo>, IGrupoRepository
    {
        public GrupoRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<Grupo> GetAtivosByEmpresa(int IdEmpresa)
        {
            var grupos = (from grupo in dbSet
                         where grupo.IdEmpresa == IdEmpresa && grupo.Excluido == false
                         orderby grupo.Nome
                         select new { Id = grupo.Id, Nome = grupo.Nome })
                         .Select(x => new Grupo { Id = x.Id, Nome = x.Nome });
            return grupos;
        }

        public IEnumerable<Grupo> GetAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome)
        {
            return Get(filter: g => g.Nome.StartsWith(startWithNome) && g.Excluido == false && g.IdEmpresa == idEmpresa).OrderBy(u => u.Nome);
        }

        public IEnumerable<Grupo> GetAtivosByEmpresaWithAtLeastOneQuestionario(int IdEmpresa)
        {
            var grupos = (from grupo in dbSet
                         where grupo.IdEmpresa == IdEmpresa && grupo.Excluido == false && grupo.Questionarios.Any(q => q.Questionario.Excluido == false)                         
                         group grupo by new { grupo.Id, grupo.Nome } into g
                         where g.Count() > 0
                         select new { Id = g.FirstOrDefault().Id, Nome = g.FirstOrDefault().Nome }).ToList()
                         .Select(x => new Grupo { Id = x.Id, Nome = x.Nome });

            return grupos;
        }
    }
}
