using System.Net.Http.Json;
using PruebaBff.Core.Models;
using PruebaBff.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PruebaBff.Infrastructure
{
    public class RickAndMortyService : IRickAndMortyService
    {
        private readonly HttpClient _http;
        public RickAndMortyService(HttpClient http)
        {
            _http = http;
        }

        public async Task<(IEnumerable<EpisodeDto> Episodes, int TotalPages, int TotalCount)> GetEpisodesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            // Rick and Morty API uses page-based pagination via ?page=
            var response = await _http.GetFromJsonAsync<ApiEpisodesResponse>($"episode?page={page}", cancellationToken);
            if (response == null) return (Enumerable.Empty<EpisodeDto>(), 0, 0);
            var episodes = response.Results.Select(e => new EpisodeDto(
                e.Id, e.Name, e.Air_date, e.Episode, e.Characters.ToArray(), e.Url, e.Created
            ));
            return (episodes, response.Info.Pages, response.Info.Count);
        }

        public async Task<EpisodeDto?> GetEpisodeByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var e = await _http.GetFromJsonAsync<EpisodeResponse>($"episode/{id}", cancellationToken);
            if (e == null) return null;
            return new EpisodeDto(e.Id, e.Name, e.Air_date, e.Episode, e.Characters.ToArray(), e.Url, e.Created);
        }

        // internal classes to map response
        private class ApiEpisodesResponse
        {
            public Info Info { get; set; } = default!;
            public EpisodeResponse[] Results { get; set; } = Array.Empty<EpisodeResponse>();
        }
        private class Info { public int Count { get; set; } public int Pages { get; set; } public string? Next { get; set; } public string? Prev { get; set; } }
        private class EpisodeResponse { public int Id { get; set; } public string Name { get; set; } = default!; public string Air_date { get; set; } = default!; public string Episode { get; set; } = default!; public List<string> Characters { get; set; } = new(); public string Url { get; set; } = default!; public string Created { get; set; } = default!; }
    }

}
