namespace WebApi.DTOs.Movies.POST
{
    public class MovieOutputPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public long DirectorId { get; set; }

        public MovieOutputPostDTO(int id, string title, string year, string genre, long directorId)
        {
            Id = id;
            Title = title;
            Year = year;
            Genre = genre;
            DirectorId = directorId;
        }
    }
}