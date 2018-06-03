using System.Threading.Tasks;

namespace CamDist.Patcher.Abstractions.Async
{
    public interface IBackupManagerAsync
    {
        Task CreateBackupAsync(string source, string destdestination, bool isOverride);
    }
}
