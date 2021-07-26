using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Movies.GET;
using WebApi.Extensions;
using WebApi.Models;

namespace WebApi.Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly Context _context;
        public MovieService(Context context)
        {
            _context = context;
        }

        public async Task<MoviesListOutputGetAllDTO> GetByPageAsync(int limit, int page, CancellationToken cancellationToken) {
            var pagedModel = await _context.Movies
                    .AsNoTracking()
                    .OrderBy(p => p.Id)
                    .PaginateAsync(page, limit, cancellationToken);

            if (!pagedModel.Items.Any()) {
                throw new Exception("Não existem filmes cadastrados!");
            }

            return new MoviesListOutputGetAllDTO {
                CurrentPage = pagedModel.CurrentPage,
                TotalPages = pagedModel.TotalPages,
                TotalItems = pagedModel.TotalItems,
                Items = pagedModel.Items.Select(movie => new MovieOutputGetDTO(movie.Id, movie.Title, movie.Year, movie.Genre, movie.DirectorId)).ToList()
            };
        }

        public async Task<Movie> GetById(long id) {
            var movie = await _context.Movies.FirstOrDefaultAsync(movie => movie.Id == id);

            if (movie == null) {
                throw new Exception("Movie not found!");
            }

            return movie;
        }

        public async Task<Movie> Create(Movie movie) {
            _context.Movies.Add(movie);                    

            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> Update(Movie movie, long id) {
            movie.Id = (int)id;
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task Delete(long id) {
            var movie = await _context.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
            _context.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}