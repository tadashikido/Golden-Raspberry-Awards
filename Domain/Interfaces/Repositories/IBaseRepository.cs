using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity obj);

        void Update(TEntity obj);

        void Remove(int id);

        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);

        TEntity Get(int id);
    }
}
