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
                var movies = await _context.Movies.ToListAsync();

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
    }
}