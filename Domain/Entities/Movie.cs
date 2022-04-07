using System.Collections.Generic;

namespace Domain.Entities
{
    public class Movie : BaseEntity
    {
        public int Year { get; set; }
        public string Title { get; set; }
        public bool Winner { get; set; }
        public List<MovieProducer> MovieProducer { get; set; }
        public List<MovieStudio> MovieStudio { get; set; }
    }
}
