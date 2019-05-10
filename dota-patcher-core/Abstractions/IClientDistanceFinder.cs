using System.Collections.Generic;
using Dota.Patcher.Core.Models;

namespace Dota.Patcher.Core.Abstractions
{
    public interface IClientDistanceFinder
    {
        IEnumerable<SearchResult<string>> Find(byte[] array, IEnumerable<byte[]> patterns);
        IEnumerable<SearchResult<string>> Find(byte[] array, byte[] pattern);
        IEnumerable<SearchResult<string>> Find(byte[] array, IEnumerable<int> offsets);
        string Find(byte[] array, int offset);
    }
}