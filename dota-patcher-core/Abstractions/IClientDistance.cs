using System.Collections.Generic;
using Dota.Patcher.Core.Models;

namespace Dota.Patcher.Core.Abstractions
{
    public interface IClientDistance
    {
        IEnumerable<SearchResult<string>> Get(byte[] array, IEnumerable<byte[]> patterns);

        IEnumerable<SearchResult<string>> Get(byte[] array, byte[] pattern);

        string Get(byte[] array, int offset);
    }
}