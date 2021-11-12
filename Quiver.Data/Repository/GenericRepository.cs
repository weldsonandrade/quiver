using Quiver.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Quiver.Data.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal QuiverDbContext context;
        internal IDbSet<TEntity> dbSet;

        public GenericRepository(QuiverDbContext context)
        {
            context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int take = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(params object[] id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity GetByID(Expression<Func<TEntity, bool>> id, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            foreach (var includeProperty in includeProperties.Split
                 (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.Single(id);
        }

        public virtual void Insert(TEntity entity)
        {
            ApplyTrimToStringFields(entity);

            dbSet.Add(entity);
        }

        public virtual void Insert(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                ApplyTrimToStringFields(entity);
                dbSet.Add(entity);
            }
        }

        private void ApplyTrimToStringFields(TEntity entity)
        {
            var propriedades = entity.GetType().GetProperties();

            foreach (var propriedade in propriedades)
            {
                if (propriedade.PropertyType.Name.Equals("String") && propriedade.GetValue(entity) != null)
                {
                    propriedade.SetValue(entity, propriedade.GetValue(entity).ToString().Trim());
                }
            }
        }

        public virtual void Delete(params object[] id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual IQueryable<TEntity> Table
        {
            get { return dbSet; }
        }
    }
}