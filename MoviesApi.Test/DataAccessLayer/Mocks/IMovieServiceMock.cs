

using MoviesAPI.Service;
using MoviesAPI.Service.Interface;

namespace MoviesApi.Test.DataAccessLayer.Mocks
{
    public class IMovieServiceMock
    {

        public static IMovieService GetMock()
        {
            return new MovieService(IMovieRepositoryMock.GetMock());
        }
    }
}
