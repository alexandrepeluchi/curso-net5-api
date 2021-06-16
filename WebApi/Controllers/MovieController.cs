using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
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
        public async Task<ActionResult<IEnumerable<Movie>>> Get()
        {
            try
            {
                var movies = await _context.Movies.Include(d => d.Director)
                                                  .ToListAsync();

                if (!movies.Any())
                {
                    return NotFound("Movies not found.");
                }

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // POST api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> Post([FromBody] Movie movie)
        {
            try
            {
                var director = await _context.Directors.FirstOrDefaultAsync(director => director.Id == movie.DirectorId);

                if (director == null) {
                    return NotFound("Director not found.");
                }

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}