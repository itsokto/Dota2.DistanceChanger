using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Async.IO.Abstractions;
using Dota2.Patcher.Core.Abstractions;
using Dota2.Patcher.Core.Models;

namespace Dota2.Patcher.Core
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

		public async Task SetAsync(string path, int distance, IEnumerable<byte[]> patterns)
		{
			var searchResults = (await GetAsync(path, patterns)).ToList();

			if (!searchResults.Any())
			{
				throw new InvalidOperationException("No entry was found.");
			}

			if (searchResults.Count > 1)
			{
				throw new InvalidOperationException("More than one entry was found.");
			}

			await SetAsync(path, distance, searchResults.First().Offset).ConfigureAwait(false);
		}

		public async Task SetAsync(string path, int distance, int offset)
		{
			var encodedDistance = Encoding.UTF8.GetBytes(distance.ToString());;

			await _asyncFile.WriteBytesAsync(path, encodedDistance, offset).ConfigureAwait(false);
		}

		public async Task<IEnumerable<SearchResult<int>>> GetAsync(string path, IEnumerable<byte[]> patterns)
		{
			var buffer = await _asyncFile.ReadBytesAsync(path).ConfigureAwait(false);
			return _clientDistance.Get(buffer, patterns);
		}

		public async Task<SearchResult<int>> GetAsync(string path, int offset)
		{
			var buffer = await _asyncFile.ReadBytesAsync(path, offset).ConfigureAwait(false);
			return _clientDistance.Get(buffer, offset);
		}
	}
}