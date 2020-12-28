using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Async.IO.Abstractions;
using Dota2.Patcher.Core.Abstractions;
using Dota2.Patcher.Core.Models;

namespace Dota2.Patcher.Core
{
	public class DotaClientDistance : IDotaClientDistance
	{
		private readonly IAsyncFile _asyncFile;

		private readonly IDotaClientDistanceParser _dotaClientDistanceParser;

		public DotaClientDistance(IAsyncFile asyncFile, IDotaClientDistanceParser dotaClientDistanceParser)
		{
			_asyncFile = asyncFile;
			_dotaClientDistanceParser = dotaClientDistanceParser;
		}

		public async Task SetAsync(string path, int distance, int offset)
		{
			var encodedDistance = Encoding.UTF8.GetBytes(distance.ToString());

			await _asyncFile.WriteBytesAsync(path, encodedDistance, offset).ConfigureAwait(false);
		}

		public async Task<IEnumerable<SearchResult<int>>> GetAsync(string path, IEnumerable<byte[]> patterns)
		{
			var buffer = await _asyncFile.ReadBytesAsync(path).ConfigureAwait(false);

			return _dotaClientDistanceParser.Get(buffer, patterns);
		}

		public async Task<SearchResult<int>> GetAsync(string path, int offset)
		{
			var buffer = await _asyncFile.ReadBytesAsync(path, offset).ConfigureAwait(false);

			return _dotaClientDistanceParser.Get(buffer, offset);
		}
	}
}