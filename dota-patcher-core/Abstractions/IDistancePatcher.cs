using System.Collections.Generic;
using System.Threading.Tasks;
using Dota.Patcher.Core.Models;

namespace Dota.Patcher.Core.Abstractions
{
    public interface IDistancePatcher
    {
        Task SetAsync(string path, string distance, IEnumerable<byte[]> patterns);

        Task SetAsync(string path, string distance, IEnumerable<int> offsets);

        Task SetAsync(string path, string distance, int offset);

        Task<IEnumerable<SearchResult<string>>> GetAsync(string path, IEnumerable<byte[]> patterns);

        Task<IEnumerable<SearchResult<string>>> GetAsync(string path, IEnumerable<int> offsets);

        Task<string> GetAsync(string path, int offset);
    }
}