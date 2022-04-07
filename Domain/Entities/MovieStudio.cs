namespace Domain.Entities
{
    public class MovieStudio : BaseEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
    }
}
