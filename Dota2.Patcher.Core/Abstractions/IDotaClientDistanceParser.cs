using System.Collections.Generic;
using Dota2.Patcher.Core.Models;

namespace Dota2.Patcher.Core.Abstractions
{
	public interface IDotaClientDistanceParser
	{
		IEnumerable<SearchResult<int>> Get(byte[] array, IEnumerable<byte[]> patterns);

		IEnumerable<SearchResult<int>> Get(byte[] array, byte[] pattern);

		SearchResult<int> Get(byte[] array, int offset);
	}
}