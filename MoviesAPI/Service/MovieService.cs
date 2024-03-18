using MoviesAPI.Dtos;
using MoviesAPI.Models;
using MoviesAPI.Repository.Interface;
using MoviesAPI.Service.Interface;

namespace MoviesAPI.Service
{
    public class MovieService : IMovieService
    {
        IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<List<AvailableGenreDto>> GetGenreList()
        {
           return await _movieRepository.GetGenreList();
        }

        public async Task<PagedMoviesDto> GetMovies(string? name, string? genre, string? sort, int page, int limit)
        {
            return await _movieRepository.GetMovies(name, genre, sort, page, limit);    
        }
    }
}
