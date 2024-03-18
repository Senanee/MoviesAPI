using MoviesAPI.Dtos;
using MoviesAPI.Models;
using System;

namespace MoviesAPI.Repository.Interface
{
    public interface IMovieRepository
    {
     
        Task<List<AvailableGenreDto>> GetGenreList();
        Task<PagedMoviesDto> GetMovies(string? name, string? genre, string? sort, int page, int limit);
    }
}
