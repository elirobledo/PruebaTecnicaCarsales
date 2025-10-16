using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PruebaBff.Core.Models;

namespace PruebaBff.Core.Interfaces
{
    public interface IRickAndMortyService
    {
        Task<(IEnumerable<EpisodeDto> Episodes, int TotalPages, int TotalCount)> GetEpisodesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<EpisodeDto?> GetEpisodeByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
