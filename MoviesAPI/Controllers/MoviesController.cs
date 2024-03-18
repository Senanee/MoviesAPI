using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Context;
using MoviesAPI.Dtos;
using MoviesAPI.Models;
using MoviesAPI.Service.Interface;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService  = movieService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetMovies(string? name, string? genre, string? sort, int page=1, int limit=100)
        {
            if(page <1 || limit > 100)
            {
                return BadRequest("Please check pageIndex and page limit");
            }
            try
            {
                Dtos.PagedMoviesDto result = await _movieService.GetMovies(name, genre, sort, page, limit);
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetGenreList()
        {

            List<AvailableGenreDto> genre =   await _movieService.GetGenreList();

            if (!genre.Any())
            {
                return NotFound();
            }

            return Ok(genre);
        }
    }
}
