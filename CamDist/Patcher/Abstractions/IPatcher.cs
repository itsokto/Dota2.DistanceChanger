using System.Collections.Generic;

namespace CamDist.Patcher.Abstractions
{
    public interface IPatcher
    {
        bool SetCustomDistance(string path, string distance, IEnumerable<byte[]> patterns);
        IDictionary<long, string> GetCurrentDistance(string path, IEnumerable<byte[]> patterns);
        void CreateBackup(string source, string destdestination, bool isOverride);
    }
}