using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using WebApplication2.Data;
using WebApplication2.Entity;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private ArrayService arrayService;
        private MovieDbContext _context;

        public WordController(MovieDbContext context)
        {
            string filePath = @"C:\Users\Yusif\Source\Repos\WebApi_hw3\WebApplication2\Notes.txt";
            arrayService = new ArrayService(filePath);
            _httpClient = new HttpClient();
            _context = context;
        }

        [HttpGet]
        [Route("getword")]
        public IActionResult GetWord()
        {
            string word = arrayService.GetWord();
            return Ok(word);
        }

        [HttpGet]
        [Route("getfilm")]
        public async Task<IActionResult> GetData()
        {
            var word = arrayService.GetWord();
            var apiUrl = $"https://www.omdbapi.com/?apikey=cde8ba76&t={word}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var film = JsonConvert.DeserializeObject<Movie>(data);

                if (film != null)
                {
                    _context.Movies.Add(film);
                    await _context.SaveChangesAsync();
                }

                return Ok(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}
