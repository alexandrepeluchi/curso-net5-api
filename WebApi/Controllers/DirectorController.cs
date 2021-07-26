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
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("director")]
    public class DirectorController : ControllerBase
    {
        private readonly Context _context;
        private readonly IDirectorService _directorService;

        public DirectorController(Context context,
                                  IDirectorService directorService)
        {
            _context = context;
            _directorService = directorService;
        }
        
        /// <summary>
        /// Busca todos os diretores
        /// </summary>
        /// <response code="200">Sucesso ao buscar todos os diretores.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="404">Não existe diretores cadastrados.</response>
        /// <response code="409">A solicitação atual conflitou com o recurso que está no servidor</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
        [HttpGet]
        public async Task<ActionResult> Get()
        {   
            var directors = await _directorService.GetAll();
            var directorsDTO = directors.Select(d => DirectorOutputGetDTO.ToDirectorDTOMap(d)).ToList();

            return Ok(directorsDTO);
        }

        /// <summary>
        /// Busca um diretor por Id
        /// </summary>
        /// <response code="200">Sucesso ao retornar o diretor por Id.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="404">Não existe diretor cadastrado com o Id informado.</response>
        /// <response code="409">A solicitação atual conflitou com o recurso que está no servidor</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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
        /// Cria um director
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /diretor
        ///     {
        ///        "name": "Martin Scorsese",
        ///     }
        ///
        /// </remarks>
        /// <returns>O diretor criado</returns>
        /// <response code="200">Sucesso ao criar um diretor.</response>
        /// <response code="201">Retorna o diretor recém criado.</response>
        /// <response code="400">Se a requisição tiver valor null.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="409">Algum campo com dados inválidos ou ja existe um diretor com este nome.</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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

        /// <summary>
        /// Atualiza um diretor
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /director
        ///     {
        ///        "id": 1,
        ///        "name": "Martin Scorsese",
        ///     }
        ///
        /// </remarks>
        /// <returns>O diretor atualizado</returns>
        /// <response code="200">Sucesso ao atualizar um diretor.</response>
        /// <response code="201">Retorna o diretor recém atualizado.</response>
        /// <response code="400">Se a requisição tiver valor null.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="409">Algum campo com dados inválidos ou ja existe um diretor com este nome.</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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

        /// <summary>
        /// Deleta um diretor
        /// </summary>
        /// <response code="200">Sucesso ao deletar o diretor por Id.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="404">Não existe diretor cadastrado com o Id informado.</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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