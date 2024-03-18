using MoviesAPI.Dtos;

namespace MoviesAPI.Service.Interface
{
    public interface IMovieService
    {
        Task<List<AvailableGenreDto>> GetGenreList();
        Task<PagedMoviesDto> GetMovies(string? name, string? genre, string? sort, int page, int limit);
    }
}
