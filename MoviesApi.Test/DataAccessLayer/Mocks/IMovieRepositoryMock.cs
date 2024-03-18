using MoviesApi.Test.DataAccessLayer.ContextMock;
using MoviesAPI.Context;
using MoviesAPI.Models;
using MoviesAPI.Repository;
using MoviesAPI.Repository.Interface;



namespace MoviesApi.Test.DataAccessLayer.Mocks
{
    public class IMovieRepositoryMock
    {
        public static IMovieRepository GetMock()
        {
            List<Movie> listMovie = GenerateTestData();
            MoviesContext dbContextMock = DbContextMock.GetMock<Movie, MoviesContext>(listMovie, x => x.Movies);
            return new MovieRepository(dbContextMock);
        }

        private static List<Movie> GenerateTestData()
        {
            List<Movie> listMovie = new();

            listMovie.Add(
    new Movie
    {
        Id = 1,
        ReleaseDate = DateTime.Parse("2021-12-15T00:00:00"),
        Title = "Spider-Man: No Way Home",
        Overview = "Peter Parker is unmasked and no longer able to separate his normal life from the high-stakes of being a super-hero. When he asks for help from Doctor Strange the stakes become even more dangerous, forcing him to discover what it truly means to be Spider-Man.",
        Popularity = (decimal?)5083.954,
        VoteCount = 8940,
        VoteAverage = (decimal?)8.3,
        OriginalLanguage = "en",
        Genre = "Action, Adventure, Science Fiction",
        PosterUrl = "https://image.tmdb.org/t/p/original/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg"
    });
            listMovie.Add(
    new Movie
    {
        Id = 2,
        ReleaseDate = DateTime.Parse("2022-03-01T00:00:00"),
        Title = "The Batman",
        Overview= "In his second year of fighting crime, Batman uncovers corruption in Gotham City that connects t his own family while facing a serial killer known as the Riddler.",
        Popularity= (decimal?)3827.658,
        VoteCount= 1151,
        VoteAverage= (decimal?)8.1,
        OriginalLanguage= "en",
        Genre= "Crime, Mystery, Thriller",
        PosterUrl= "https://image.tmdb.org/t/p/original/74xTEgt7R36Fpooo50r9T25onhq.jpg"
    });
            return listMovie;
        }

    }
}
