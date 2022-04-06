namespace Domain.Entities
{
    public class Movie : BaseEntity
    {
        public int Year { get; set; }
        public string Title { get; set; }
        public bool Winner { get; set; }
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
    }
}
