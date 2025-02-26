using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Entity;
using WebApplication2.Services;

public class MovieBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly ArrayService _arrayService;

    public MovieBackgroundService( IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
        _arrayService = new ArrayService(@"C:\Users\Yusif\Source\Repos\WebApi_hw3\WebApplication2\Notes.txt");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GetMovie();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task GetMovie()
    {
        var word = _arrayService.GetWord();
        var apiUrl = $"https://www.omdbapi.com/?apikey=cde8ba76&t={word}";

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            var film = JsonConvert.DeserializeObject<Movie>(data);

            if (film != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
                    dbContext.Movies.Add(film);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
