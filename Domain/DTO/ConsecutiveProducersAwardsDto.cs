using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ConsecutiveProducersAwardsDto
    {
        public List<ProducerDto> Min { get; set; }
        public List<ProducerDto> Max { get; set; }
    }

    public class ProducerDto
    {
        public string Producer { get; set; }
        public int Interval { get; set; }
        public int PreviousWin { get; set; }
        public int FollowingWin { get; set; }
    }
}
