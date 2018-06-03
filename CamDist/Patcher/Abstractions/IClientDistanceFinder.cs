using System.Collections.Generic;

namespace CamDist.Patcher.Abstractions
{
    public interface IClientDistanceFinder
    {
        IDictionary<long, string> Find(byte[] array, IEnumerable<byte[]> patterns);
    }
}