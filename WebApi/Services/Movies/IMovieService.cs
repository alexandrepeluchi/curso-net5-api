using System.Threading;
using System.Threading.Tasks;
using WebApi.DTOs.Movies.GET;

namespace WebApi.Services.Movies
{
    public interface IMovieService
    {
        Task<MoviesListOutputGetAllDTO> GetByPageAsync(int limit, int page, CancellationToken cancellationToken);
    }
}