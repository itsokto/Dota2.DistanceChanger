using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Async.IO.Abstractions;
using Dota.Patcher.Core.Abstractions;
using Dota.Patcher.Core.Models;

namespace Dota.Patcher.Core
{
    public class DistancePatcher : IDistancePatcher
    {
        private readonly IAsyncFile _asyncFile;
        private readonly IClientDistance _clientDistance;

        public DistancePatcher(IAsyncFile asyncFile, IClientDistance clientDistance)
        {
            _asyncFile = asyncFile;
            _clientDistance = clientDistance;
        }

        public async Task SetAsync(string path, string distance, IEnumerable<byte[]> patterns)
        {
            var searchResults = await GetAsync(path, patterns);

            if (!searchResults.Any())
            {
                throw new InvalidOperationException("No entry was found.");
            }

            if (searchResults.Count() > 1)
            {
                throw new IndexOutOfRangeException("More than one entry was found.");
            }

            await SetAsync(path, distance, searchResults.FirstOrDefault().Offset);
        }

        public async Task SetAsync(string path, string distance, int offset)
        {
            var encodedDistance = Encoding.UTF8.GetBytes(distance);

            await _asyncFile.WriteBytesAsync(path, encodedDistance, offset);
        }

        public async Task<IEnumerable<SearchResult<string>>> GetAsync(string path, IEnumerable<byte[]> patterns)
        {
            var buffer = await _asyncFile.ReadBytesAsync(path);
            return _clientDistance.Get(buffer, patterns);
        }

        public async Task<string> GetAsync(string path, int offset)
        {
            var buffer = await _asyncFile.ReadBytesAsync(path);
            return _clientDistance.Get(buffer, offset);
        }
    }
}