using System.Collections.Generic;

namespace Dota2.DistanceChanger.Patcher.Abstractions
{
    public interface IClientDistanceFinder
    {
        IDictionary<long, string> Get(byte[] array, IEnumerable<byte[]> patterns);
        IDictionary<long, string> Get(byte[] array, byte[] pattern);
        IDictionary<long, string> Get(byte[] array, IEnumerable<long> offsets);
        string Get(byte[] array, long offset);
    }
}