using Humanizer.Localisation;
using MoviesApi.Test.DataAccessLayer.Mocks;
using MoviesAPI.Service.Interface;

namespace MoviesApi.Test
{
    public class MovieServiceTest
    {
        IMovieService _movieService;


        [SetUp]
        public void SetUp()
        {
            _movieService = IMovieServiceMock.GetMock();
        }

        [Test]
        public async Task GetMoviesWithFilter()
        {
            //Arrange
            //Act
            var data = await _movieService.GetMovies("Home", "Action", "Title,-ReleaseDate", 1, 5);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(data, Is.Not.Null);
                Assert.That(data.TotalCount, Is.GreaterThan(1));
            });
        }

        [Test]
        public async Task GetMoviesOrderBy()
        {
            //Arrange
            //Act
            var data = await _movieService.GetMovies("","","Title,-ReleaseDate",1,5);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(data, Is.Not.Null);
                Assert.That(data.TotalCount, Is.GreaterThan(1));
            });
        }

        //[Test]
        //public async Task GetGenre()
        //{
        //    var genre = await _movieService.GetGenreList();
        //    //Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(genre, Is.Not.Null);
        //        Assert.That(genre?.Count, Is.GreaterThan(1));
        //    });
        //}

    }
}
