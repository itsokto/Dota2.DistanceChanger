using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Patcher.Abstractions
{
    public interface IPatcher
    {
        Task SetDistanceAsync(string path, string distance, IEnumerable<byte[]> patterns);
        Task SetDistanceAsync(string path, string distance, IEnumerable<long> offsets);
        Task SetDistanceAsync(string path, string distance, long offsets);
        Task<IDictionary<long, string>> GetDistanceAsync(string path, IEnumerable<byte[]> patterns);
        Task<IDictionary<long, string>> GetDistanceAsync(string path, IEnumerable<long> offsets);
        Task<string> GetDistanceAsync(string path, long offset);
        Task CreateBackupAsync(string source, string destination);
    }
}