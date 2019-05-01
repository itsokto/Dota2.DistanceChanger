using System.IO;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Patcher.Abstractions;
using Dota2.DistanceChanger.Patcher.Abstractions.Async;

namespace Dota2.DistanceChanger.Patcher
{
    public class BackupManager : IBackupManager
    {
        private readonly IFileIO _fileIo;

        public BackupManager(IFileIO fileIo)
        {
            _fileIo = fileIo;
        }

        public void CreateBackup(string source, string destination)
        {
            CreateBackupAsync(source, destination).GetAwaiter().GetResult();
        }

        public async Task CreateBackupAsync(string source, string destination)
        {
            if (File.Exists(source)) await _fileIo.CopyFileAsync(source, destination, 81920);
        }
    }
}