using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Movies.GET;
using WebApi.DTOs.Movies.POST;
using WebApi.DTOs.Movies.PUT;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly Context _context;

        public MovieController(Context context)
        {
            _context = context;
        }

        // GET api/movies
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            
            var movies = await _context.Movies.Include(d => d.Director)
                                              .ToListAsync();

            if (!movies.Any())
            {
                return NotFound("Movies not found.");
            }

            var moviesDTO = movies.Select(d => MovieOutputGetDTO.ToMovieDTOMap(d)).ToList();

            return Ok(moviesDTO);
        }

        // GET api/movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var movie = await _context.Movies.Include(m => m.Director)
                                                .FirstOrDefaultAsync(movie => movie.Id == id);

            if (movie == null)
            {
                return NotFound("There are no registered Movies.");
            }

            var movieOutputDTO = new MovieOutputGetDTO(movie.Id,
                                                        movie.Title,
                                                        movie.Year,
                                                        movie.Genre,
                                                        movie.DirectorId);

            return Ok(movieOutputDTO);
        }

        // POST api/movies
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MovieInputPostDTO movieInputPostDTO)
        {
            var director = await _context.Directors.FirstOrDefaultAsync(director => director.Id == movieInputPostDTO.DirectorId);

            if (director == null) 
            {
                return NotFound("Director not found.");
            }

            var movie = new Movie(movieInputPostDTO.Title,
                                    movieInputPostDTO.Year,
                                    movieInputPostDTO.Genre,
                                    movieInputPostDTO.DirectorId);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var movieOutputPostDTO = new MovieOutputPostDTO(movie.Id,
                                                            movie.Title,
                                                            movie.Year,
                                                            movie.Genre,
                                                            movie.DirectorId);

            return Ok(movieOutputPostDTO);
        }

        // PUT api/movies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MovieInputPutDTO movieInputPutDTO)
        {
            movieInputPutDTO.Id = id;

            var movie = new Movie(movieInputPutDTO.Id,
                                    movieInputPutDTO.Title,
                                    movieInputPutDTO.Year,
                                    movieInputPutDTO.Genre,
                                    movieInputPutDTO.DirectorId);

            if (movieInputPutDTO.DirectorId == 0) 
            {
                return NotFound("Director Id is invalid.");
            }

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            var movieOutputPutDTO = new MovieOutputPutDTO(movie.Id,
                                                            movie.Title,
                                                            movie.Year,
                                                            movie.Genre,
                                                            movie.DirectorId);

            return Ok(movieOutputPutDTO);
        }

        // DELETE api/movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
            _context.Remove(movie);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}