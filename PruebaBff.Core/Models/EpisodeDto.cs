using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaBff.Core.Models
{
    public record EpisodeDto(
        int Id,
        string Name,
        string AirDate,
        string Episode,
        string[] Characters,
        string Url,
        string Created
    );
}
