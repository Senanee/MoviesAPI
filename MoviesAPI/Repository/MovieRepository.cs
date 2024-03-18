using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Context;
using MoviesAPI.Dtos;
using MoviesAPI.Models;
using MoviesAPI.Repository.Interface;
using System;
using System.Reflection;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;
using System.Linq.Dynamic.Core;

namespace MoviesAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesContext _dataContext;

        public MovieRepository(MoviesContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<AvailableGenreDto>> GetGenreList()
        {

            if (_dataContext.Movies == null)
                return new List<AvailableGenreDto>();

            var ret = await _dataContext.AvailableGenres.Select(x=> new AvailableGenreDto { Genre=x.Genre}).ToListAsync();
            return ret;
        }

        public async Task<PagedMoviesDto> GetMovies(string? name, string? genre, string? sort, int page, int limit)
        {
            if (_dataContext.Movies == null)
                return new PagedMoviesDto();

            IQueryable<Movie> movies;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(genre))
            {
                name = name.Trim().ToLower();
                genre = genre.Trim().ToLower();
                movies = _dataContext
                    .Movies
                    .Where(b => b.Title.ToLower().Contains(name) && !string.IsNullOrWhiteSpace(b.Genre) && b.Genre.ToLower().Contains(genre));
            }
            else if (!string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(genre))
            {
                name = name.Trim().ToLower();
                movies = _dataContext
                    .Movies
                    .Where(b => b.Title.ToLower().Contains(name));
            }
            else if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(genre))
            {
                genre = genre.Trim().ToLower();
                movies = _dataContext
                    .Movies
                    .Where(b => !string.IsNullOrWhiteSpace(b.Genre) && b.Genre.ToLower().Contains(genre));
            }
            else
            {
                movies = _dataContext.Movies;
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var sortFields = sort.Split(','); // ['title','-releaseDate']
                StringBuilder orderQueryBuilder = new StringBuilder();
                PropertyInfo[] propertyInfo = typeof(Movie).GetProperties();


                foreach (var field in sortFields)
                {

                    string sortOrder = "ascending";

                    var sortField = field.Trim();
                    if (sortField.StartsWith("-"))
                    {
                        sortField = sortField.TrimStart('-');
                        sortOrder = "descending";
                    }
                    var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                        continue;

                    orderQueryBuilder.Append($"{property.Name.ToString()} {sortOrder}, ");
                }

                string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
                if (!string.IsNullOrWhiteSpace(orderQuery))
                    // use System.Linq.Dynamic.Core namespace for this
                    movies = movies.OrderBy(orderQuery);
                else
                    movies = movies.OrderBy(a => a.Title);
            }


            var totalCount = await movies.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var pagedMovies = await movies.Skip((page - 1) * limit).Take(limit).Select(x => new MovieDto { Title = x.Title, Id = x.Id, ReleaseDate = x.ReleaseDate, Overview = x.Overview, Popularity = x.Popularity, VoteCount = x.VoteCount, VoteAverage = x.VoteAverage, OriginalLanguage = x.OriginalLanguage, Genre = x.Genre, PosterUrl = x.PosterUrl }).ToListAsync();

            var pagedMovieData = new PagedMoviesDto
            {
                Movies = pagedMovies,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            
            return pagedMovieData;
        }
    }
}
