namespace WebApi.DTOs.Movies.PUT
{
    public class MovieOutputPutDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public long DirectorId { get; set; }

        public MovieOutputPutDTO(int id, string title, string year, string genre, long directorId)
        {
            Id = id;
            Title = title;
            Year = year;
            Genre = genre;
            DirectorId = directorId;
        }
    }
}