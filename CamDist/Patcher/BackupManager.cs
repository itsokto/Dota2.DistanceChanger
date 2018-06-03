using System.IO;
using System.Threading.Tasks;
using CamDist.Patcher.Abstractions;

namespace CamDist.Patcher
{
    public class BackupManager : IBackupManager
    {
        public void CreateBackup(string source, string destdestination, bool isOverride)
        {
            if (File.Exists(source))
            {
                File.Copy(source, destdestination, isOverride);
            }
        }

        public Task CreateBackupAsync(string source, string destdestination, bool isOverride)
        {
            return Task.Run(() =>
            {
                CreateBackup(source, destdestination, isOverride);
            });
        }
    }
}
