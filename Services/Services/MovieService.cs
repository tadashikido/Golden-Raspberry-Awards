using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MovieService : BaseService, IMovieService
    {
        public MovieService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public ConsecutiveProducersAwardsDto GetConsecutiveProducersAwards()
        {
            var producers = _unitOfWork.Movies.GetAll(x => x.Winner).ToList()
                .GroupBy(x => x.Producer)
                .Select(g => new
                {
                    Producer = g.Key,
                    Movies = g.OrderBy(x => x.Year).ToList(),
                })
                .Where(x => x.Movies.Count > 1)
                .Select(x => new
                {
                    Movies = x.Movies.Select((y, index) => new ProducerDto
                    {
                        Producer = x.Producer.Name,
                        Interval = index > 0 ? y.Year - x.Movies[index - 1].Year : 0,
                        PreviousWin = index > 0 ? x.Movies[index - 1].Year : 0,
                        FollowingWin = y.Year,
                    })
                    .Where(x => x.PreviousWin > 0)
                })
                .SelectMany(x => x.Movies)
                .ToList();

            var min = producers.Where(x => x.Interval == producers.Min(y => y.Interval)).ToList();
            var max = producers.Where(x => x.Interval == producers.Max(y => y.Interval)).ToList();

            return new ConsecutiveProducersAwardsDto
            {
                Min = min,
                Max = max
            };
        }
    }
}
