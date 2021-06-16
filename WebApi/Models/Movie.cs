namespace WebApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public long DirectorId { get; set; }
        public Director Director { get; set; }

        public Movie(string title)
        {
            Title = title;
        }
    }
}