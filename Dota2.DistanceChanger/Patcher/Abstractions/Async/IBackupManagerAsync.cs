using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Patcher.Abstractions.Async
{
    public interface IBackupManagerAsync
    {
        Task CreateBackupAsync(string source, string destination);
    }
}