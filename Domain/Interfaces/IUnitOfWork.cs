using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        IMovieProducerRepository MovieProducers { get; }

        int SaveChanges();
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
