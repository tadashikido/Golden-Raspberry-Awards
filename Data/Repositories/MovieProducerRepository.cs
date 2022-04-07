using Data.Context;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class MovieProducerRepository : BaseRepository<MovieProducer>, IMovieProducerRepository
    {
        public MovieProducerRepository(BaseContext baseContext) : base(baseContext) { }

        private IQueryable<MovieProducer> GetIncludedQueryable()
        {
            return _dbSet.Include(x => x.Movie)
                         .Include(x => x.Producer);
        }

        public override MovieProducer Get(int id)
        {
            var query = GetIncludedQueryable();

            return query.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IList<MovieProducer> GetAll(Expression<Func<MovieProducer, bool>> filter = null)
        {
            var query = GetIncludedQueryable();

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }
    }
}
