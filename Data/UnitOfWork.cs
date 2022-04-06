using Data.Context;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly BaseContext _db;
        private IDbContextTransaction _transaction;

        public UnitOfWork(BaseContext ceContext)
        {
            _db = ceContext;

            Movies = new MovieRepository(ceContext);
        }

        public IMovieRepository Movies { get; private set; }

        public void BeginTransaction()
        {
            _transaction = _db.Database.BeginTransaction();
        }

        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch
            {
                _db.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
                throw;
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _db.Dispose();
            _transaction?.Dispose();
        }
    }
}
