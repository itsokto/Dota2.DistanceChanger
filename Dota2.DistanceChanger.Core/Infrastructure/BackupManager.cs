using System.IO;
using System.Threading.Tasks;
using Async.IO.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;

namespace Dota2.DistanceChanger.Core.Infrastructure
{
    public class BackupManager : IBackupManager
    {
        private readonly IAsyncFile _asyncFile;

        public BackupManager(IAsyncFile asyncFile)
        {
            _asyncFile = asyncFile;
        }

        public async Task CreateBackupAsync(string source, string destination)
        {
            if (File.Exists(source)) await _asyncFile.CopyFileAsync(source, destination);
        }
    }
}