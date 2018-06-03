using CamDist.Patcher.Abstractions.Async;

namespace CamDist.Patcher.Abstractions
{
    public interface IBackupManager : IBackupManagerAsync
    {
        void CreateBackup(string source, string destdestination, bool isOverride);
    }
}