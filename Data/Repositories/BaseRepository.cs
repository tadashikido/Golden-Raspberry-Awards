using Data.Context;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly BaseContext _baseContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(BaseContext baseContext)
        {
            _baseContext = baseContext;
            _dbSet = _baseContext.Set<TEntity>();
        }

        public void Add(TEntity obj)
        {
            _dbSet.Add(obj);
        }

        public void Update(TEntity obj)
        {
            _baseContext.Entry(obj).State = EntityState.Modified;
        }

        public void Remove(int id)
        {
            _dbSet.Remove(Get(id));
        }

        public virtual IList<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null) =>
            filter != null ? _dbSet.Where(filter).ToList() : _dbSet.ToList();

        public virtual TEntity Get(int id) =>
            _baseContext.Set<TEntity>().Find(id);
    }
}
