namespace WebApi.DTOs.Movies.POST
{
    public class MovieInputPostDTO
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public long DirectorId { get; set; }

        public MovieInputPostDTO(string title, string year, string genre, long directorId)
        {
            Title = title;
            Year = year;
            Genre = genre;
            DirectorId = directorId;
        }
    }
}