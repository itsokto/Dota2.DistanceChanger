using System.Collections.Generic;
using System.Threading.Tasks;
using Dota2.Patcher.Core.Models;

namespace Dota2.Patcher.Core.Abstractions
{
	public interface IDotaClientDistance
	{
		Task SetAsync(string path, int distance, int offset);

		Task<IEnumerable<SearchResult<int>>> GetAsync(string path, IEnumerable<byte[]> patterns);

		Task<SearchResult<int>> GetAsync(string path, int offset);
	}
}