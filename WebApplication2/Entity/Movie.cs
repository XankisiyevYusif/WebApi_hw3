using Newtonsoft.Json;

namespace WebApplication2.Entity
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [JsonProperty("Plot")]
        public string? Description { get; set; }
        public int Year { get; set; }
        public string? Genre { get; set; }
        public string? Rated { get; set; }
    }
}
