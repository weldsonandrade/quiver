using Quiver.Core.Models;
using System.Collections.Generic;
using System.Linq;
using Quiver.Data.Interfaces;
using System.Diagnostics;
using System;

namespace Quiver.Data.Repository
{
    public class UnidadeRepository : GenericRepository<Unidade>, IUnidadeRepository
    {
        public UnidadeRepository(QuiverDbContext context) : base(context) { }

        public IEnumerable<Unidade> GetAtivosByEmpresa(int IdEmpresa)
        {
            var unidades = (from unidade in dbSet
                            where unidade.IdEmpresa == IdEmpresa && unidade.Excluido == false
                            orderby unidade.Nome
                            select new { Id = unidade.Id, Nome = unidade.Nome }).ToList()
                           .Select(x => new Unidade { Id = x.Id, Nome = x.Nome });
            return unidades;
        }

        public IEnumerable<Unidade> GetAtivosByEmpresaAndStartWithNome(int IdEmpresa, string startWithNome)
        {
            var unidades = (from unidade in dbSet
                            where unidade.IdEmpresa == IdEmpresa && unidade.Excluido == false
                            && unidade.Nome.StartsWith(startWithNome)
                            orderby unidade.Nome
                            select new { Id = unidade.Id, Nome = unidade.Nome }).ToList()
                           .Select(x => new Unidade { Id = x.Id, Nome = x.Nome });
            return unidades;
        }

        public int GetIdEmpresaByIdUnidade(int idUnidade)
        {
            return (from unidade in dbSet where unidade.Id == idUnidade select unidade.IdEmpresa).FirstOrDefault();
        }
    }
}
