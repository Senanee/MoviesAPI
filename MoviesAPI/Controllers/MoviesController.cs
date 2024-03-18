using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesController(MoviesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]")]
        public  IActionResult GetMovies()
        {
            return Ok(  _context.Movies.AsQueryable());
        }


        [HttpGet]
        
        [Route("[action]")]
        public async Task<IActionResult> GetMovie([FromQuery] int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }
        
        [HttpGet]
        [EnableQuery]
        [Route("[action]")]
        public async Task<IActionResult> GetMovieByName([FromQuery] string name)
        {
            List<Movie> movie = await _context.Movies.Where(x=>x.Title.ToLower().Contains(name.ToLower())).ToListAsync();

            if (movie.Count == 0)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]")]
        public async Task<IActionResult> GetMovieByGenre([FromQuery] string genre)
        {
            List<Movie> movie = await _context.Movies.Where(x => x.Title.ToLower().Contains(genre.ToLower())).ToListAsync();

            if (movie.Count == 0)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetGenreList()
        {

            List<AvailableGenre> genre =   await _context.AvailableGenres.ToListAsync();

            if (!genre.Any())
            {
                return NotFound();
            }

            return Ok(genre);
        }
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
