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

        public Movie(string title, long directorId)
        {
            Title = title;
            DirectorId = directorId;
        }

        public Movie(int id, string title, string year, string genre, long directorId)
        {
            Id = id;
            Title = title;
            Year = year;
            Genre = genre;
            DirectorId = directorId;
        }

        public Movie(string title, string year, string genre, long directorId)
        {
            Title = title;
            Year = year;
            Genre = genre;
            DirectorId = directorId;
        }
    }
}