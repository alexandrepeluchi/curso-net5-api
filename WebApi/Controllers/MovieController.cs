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
    [Route("movie")]
    public class MovieController : ControllerBase
    {
        private readonly Context _context;

        public MovieController(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca todos os filmes
        /// </summary>
        /// <response code="200">Sucesso ao buscar todos os filmes.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="404">Não existe filmes cadastrados.</response>
        /// <response code="409">A solicitação atual conflitou com o recurso que está no servidor</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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

        /// <summary>
        /// Busca um filme por Id
        /// </summary>
        /// <response code="200">Sucesso ao retornar o filme por Id.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="404">Não existe filme cadastrado com o Id informado.</response>
        /// <response code="409">A solicitação atual conflitou com o recurso que está no servidor</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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

        /// <summary>
        /// Cria um filme
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /movie
        ///     {
        ///        "title": "Pulp Fiction",
        ///        "year": "1994",
        ///        "genre": "Drama",
        ///        "directorId": 1,
        ///     }
        ///
        /// </remarks>
        /// <returns>O filme criado</returns>
        /// <response code="200">Sucesso ao criar um filme.</response>
        /// <response code="201">Retorna o filme recém criado.</response>
        /// <response code="400">Se a requisição tiver valor null.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="409">Algum campo com dados inválidos ou ja existe um filme com este nome.</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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

        /// <summary>
        /// Atualiza um filme
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /movie
        ///     {
        ///        "id": 1,
        ///        "title": "Pulp Fiction",
        ///        "year": "1994",
        ///        "genre": "Drama",
        ///        "directorId": 1,
        ///     }
        ///
        /// </remarks>
        /// <returns>O filme atualizado</returns>
        /// <response code="200">Sucesso ao atualizar um filme.</response>
        /// <response code="201">Retorna o filme recém atualizado.</response>
        /// <response code="400">Se a requisição tiver valor null.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="409">Algum campo com dados inválidos ou ja existe um filme com este nome.</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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

        /// <summary>
        /// Deleta um filme
        /// </summary>
        /// <response code="200">Sucesso ao deletar o filme por Id.</response> 
        /// <response code="403">Você não tem permissão de acesso nesse servidor.</response>
        /// <response code="404">Não existe filme cadastrado com o Id informado.</response>
        /// <response code="500">A solicitação não foi concluída devido a um erro interno no lado do servidor.</response>
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