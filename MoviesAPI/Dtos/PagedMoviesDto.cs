namespace MoviesAPI.Dtos
{
    public class PagedMoviesDto
    {
        public IEnumerable<MovieDto>? Movies { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
