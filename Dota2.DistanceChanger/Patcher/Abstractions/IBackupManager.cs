using Dota2.DistanceChanger.Patcher.Abstractions.Async;

namespace Dota2.DistanceChanger.Patcher.Abstractions
{
    public interface IBackupManager : IBackupManagerAsync
    {
        void CreateBackup(string source, string destination);
    }
}