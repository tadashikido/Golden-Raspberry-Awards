using System.Collections.Generic;

namespace Domain.Entities
{
    public class Producer : BaseEntity
    {
        public string Name { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
