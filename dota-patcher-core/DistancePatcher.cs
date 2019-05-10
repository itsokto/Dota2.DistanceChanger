using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Async.IO;
using Async.IO.Abstractions;
using Dota.Patcher.Core.Abstractions;
using Dota.Patcher.Core.Models;

namespace Dota.Patcher.Core
{
    public class DistancePatcher : IDistancePatcher
    {
        private readonly IAsyncFile _asyncFile;
        private readonly IClientDistanceFinder _clientDistanceFinder;

        // ReSharper disable once InconsistentNaming
        public DistancePatcher(IAsyncFile asyncFile, IClientDistanceFinder clientDistanceFinder)
        {
            _asyncFile = asyncFile;
            _clientDistanceFinder = clientDistanceFinder;
        }

        public async Task SetAsync(string path, string distance, IEnumerable<byte[]> patterns)
        {
            var result = await GetAsync(path, patterns);

            await SetAsync(path, distance, result.Select(x => x.Offset));
        }

        public async Task SetAsync(string path, string distance, IEnumerable<int> offsets)
        {
            var encodedDistance = Encoding.Default.GetBytes(distance);
            foreach (var offset in offsets)
                await _asyncFile.WriteBytesAsync(path, encodedDistance, offset);
        }

        public Task SetAsync(string path, string distance, int offset)
        {
            return SetAsync(path, distance, new[] {offset});
        }

        public async Task<IEnumerable<SearchResult<string>>> GetAsync(string path, IEnumerable<byte[]> patterns)
        {
            var buffer = await _asyncFile.ReadBytesAsync(path);
            var dictionary = _clientDistanceFinder.Find(buffer, patterns);
            return dictionary;
        }

        public async Task<IEnumerable<SearchResult<string>>> GetAsync(string path, IEnumerable<int> offsets)
        {
            var buffer = await _asyncFile.ReadBytesAsync(path);
            var dictionary = _clientDistanceFinder.Find(buffer, offsets);
            return dictionary;
        }

        public async Task<string> GetAsync(string path, int offset)
        {
            var buffer = await _asyncFile.ReadBytesAsync(path);
            return _clientDistanceFinder.Find(buffer, offset);
        }
    }
}