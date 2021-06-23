using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Director.POST;
using WebApi.DTOs.Director.PUT;
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
            try
            {
                var directors = await _context.Directors.Include(d => d.Movies)
                                           .ToListAsync();

                if (!directors.Any())
                {
                    return NotFound("Directors not found.");
                }

                return Ok(directors);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        //GET api/directors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(long id)
        {
            try
            {
                var director = await _context.Directors.Include(m => m.Movies)
                                                  .FirstOrDefaultAsync(director => director.Id == id);

                if (director == null)
                {
                    return NotFound("Director not found.");
                }

                var directorOutputDTO = new DirectorOutputPostDTO(director.Id, director.Name);

                return Ok(directorOutputDTO);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        //POST api/directors/
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DirectorInputPostDTO directorInputDTO)
        {
            try
            {
                if (directorInputDTO.Name == null || directorInputDTO.Name == "")
                {
                    return Conflict("Director can't be empty!");
                }

                var director = new Director(directorInputDTO.Name);
                _context.Directors.Add(director);
                await _context.SaveChangesAsync();

                var directorOutputDTO = new DirectorOutputPostDTO(director.Id, director.Name);

                return Ok(directorOutputDTO);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Put api/directors/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(long id, [FromBody] DirectorInputPutDTO directorInputPutDTO)
        {
            try
            {
                var director = new Director(directorInputPutDTO.Name);
                director.Id = id;

                _context.Directors.Update(director);
                await _context.SaveChangesAsync();

                var directorOutputPutDTO = new DirectorOutputPutDTO(director.Id, director.Name);

                return Ok(directorOutputPutDTO);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var director = await _context.Directors.FirstOrDefaultAsync(director => director.Id == id);
                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();

                var directorOutputPutDTO = new DirectorOutputPutDTO(director.Id, director.Name);

                return Ok(directorOutputPutDTO);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}