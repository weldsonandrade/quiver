using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Quiver.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int take = 0);

        TEntity GetByID(params object[] id);

        TEntity GetByID(Expression<Func<TEntity, bool>> id, string includeProperties = "");

        void Insert(TEntity entity);

        void Insert(List<TEntity> entity);

        void Delete(params object[] id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        IQueryable<TEntity> Table { get; }
    }
}
