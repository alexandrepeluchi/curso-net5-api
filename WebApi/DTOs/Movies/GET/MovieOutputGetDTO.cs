using WebApi.Models;

namespace WebApi.DTOs.Movies.GET
{
    public class MovieOutputGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public long DirectorId { get; set; }

        public MovieOutputGetDTO(int id, string title, string year, string genre, long directorId)
        {
            Id = id;
            Title = title;
            Year = year;
            Genre = genre;
            DirectorId = directorId;
        }

        public static MovieOutputGetDTO ToMovieDTOMap(Movie movie)
        {
            return new MovieOutputGetDTO(movie.Id,
                                         movie.Title,
                                         movie.Year,
                                         movie.Genre,
                                         movie.DirectorId);
        }
    }
}