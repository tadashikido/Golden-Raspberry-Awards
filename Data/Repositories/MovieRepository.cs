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
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(BaseContext baseContext) : base(baseContext) { }

        private IQueryable<Movie> GetIncludedQueryable()
        {
            return _dbSet.Include(x => x.Producer)
                         .Include(x => x.Studio);
        }

        public override Movie Get(int id)
        {
            var query = GetIncludedQueryable();

            return query.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IList<Movie> GetAll(Expression<Func<Movie, bool>> filter = null)
        {
            var query = GetIncludedQueryable();

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }
    }
}
