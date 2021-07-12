using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Directors.GET;
using WebApi.DTOs.Directors.POST;
using WebApi.DTOs.Directors.PUT;
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
        public async Task<ActionResult> Get()
        {   
            var directors = await _context.Directors.Include(d => d.Movies)
                                                    .ToListAsync();

            if (!directors.Any())
            {
                return NotFound("There are no registered Directors.");
            }

            var directorsDTO = directors.Select(d => DirectorOutputGetDTO.ToDirectorDTOMap(d)).ToList();

            return Ok(directorsDTO);
        }

        //GET api/directors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(long id)
        {
            var director = await _context.Directors.Include(m => m.Movies)
                                                    .FirstOrDefaultAsync(director => director.Id == id);

            if (director == null)
            {
                return NotFound("Director not found.");
            }

            var directorOutputDTO = new DirectorOutputGetDTO(director.Id, director.Name);

            return Ok(directorOutputDTO);
        }

        /// <summary>
        /// Cria um diretor
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /diretor
        ///     {
        ///        "nome": "Martin Scorsese",
        ///     }
        ///
        /// </remarks>
        /// <returns>O diretor criado</returns>
        /// <response code="200">Diretor foi criado com sucesso</response>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DirectorInputPostDTO directorInputPostDTO)
        {
            if (directorInputPostDTO.Name == null || directorInputPostDTO.Name == "")
            {
                return Conflict("Director can't be empty!");
            }

            var director = new Director(directorInputPostDTO.Name);
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            var directorOutputDTO = new DirectorOutputPostDTO(director.Id, director.Name);

            return Ok(directorOutputDTO);
        }

        // Put api/directors/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(long id, [FromBody] DirectorInputPutDTO directorInputPutDTO)
        {
            var director = new Director(directorInputPutDTO.Name);
            director.Id = id;

            _context.Directors.Update(director);
            await _context.SaveChangesAsync();

            var directorOutputPutDTO = new DirectorOutputPutDTO(director.Id, director.Name);

            return Ok(directorOutputPutDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var director = await _context.Directors.FirstOrDefaultAsync(director => director.Id == id);
            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            var directorOutputPutDTO = new DirectorOutputPutDTO(director.Id, director.Name);

            return Ok(directorOutputPutDTO);
        }
    }
}