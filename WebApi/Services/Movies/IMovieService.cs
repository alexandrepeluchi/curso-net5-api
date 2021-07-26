using System.Threading;
using System.Threading.Tasks;
using WebApi.DTOs.Movies.GET;
using WebApi.Models;

namespace WebApi.Services.Movies
{
    public interface IMovieService
    {
        Task<Movie> Create(Movie movie);
        Task Delete(long id);
        Task<Movie> GetById(long id);
        Task<MoviesListOutputGetAllDTO> GetByPageAsync(int limit, int page, CancellationToken cancellationToken);
        Task<Movie> Update(Movie movie, long id);
    }
}