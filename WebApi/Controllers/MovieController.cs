using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Movies.GET;
using WebApi.DTOs.Movies.POST;
using WebApi.DTOs.Movies.PUT;
using WebApi.Models;
using WebApi.Services.Movies;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("movie")]
    public class MovieController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMovieService _movieService;


        public MovieController(Context context,
                               IMovieService movieService)
        {
            _context = context;
            _movieService = movieService;
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
        public async Task<ActionResult<MoviesListOutputGetAllDTO>> Get(CancellationToken cancellationToken, int limit = 5, int page = 1) 
        {
            return await _movieService.GetByPageAsync(limit, page, cancellationToken);        
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
            var movie = await _movieService.GetById(id);

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
            var movie = await _movieService.Create(new Movie(movieInputPostDTO.Title, movieInputPostDTO.DirectorId));

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
            var movie = new Movie(id,
                                  movieInputPutDTO.Title,
                                  movieInputPutDTO.Year,
                                  movieInputPutDTO.Genre,
                                  movieInputPutDTO.DirectorId);

            await _movieService.Update(movie, movie.Id);

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
            await _movieService.Delete(id);

            return Ok();
        }
    }
}