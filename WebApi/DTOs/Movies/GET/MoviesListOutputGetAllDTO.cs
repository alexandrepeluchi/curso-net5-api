using System.Collections.Generic;

namespace WebApi.DTOs.Movies.GET
{
    public class MoviesListOutputGetAllDTO
    {
        public int CurrentPage { get; init; }

        public int TotalItems { get; init; }

        public int TotalPages { get; init; }

        public List<MovieOutputGetDTO> Items { get; init; }
    }
}