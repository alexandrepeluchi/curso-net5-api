using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly Context _context;

        public DirectorController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Director>> Get()
        {
            return await _context.Directors.Include(m => m.Movies)
                                           .ToListAsync();
        }

        //POST api/diretores/
        [HttpPost]
        public async Task<ActionResult<Director>> Post([FromBody] Director director)
        {
            if (director.Name == null || director.Name == "")
            {
                return Conflict("Director can't be empty!");
            }
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            return Ok(director);
        }
    }
}