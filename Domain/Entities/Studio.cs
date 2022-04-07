using System.Collections.Generic;

namespace Domain.Entities
{
    public class Studio : BaseEntity
    {
        public string Name { get; set; }

        public List<MovieStudio> MovieStudio { get; set; }
    }
}
