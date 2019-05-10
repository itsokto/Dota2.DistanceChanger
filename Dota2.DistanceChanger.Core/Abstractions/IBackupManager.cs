using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Abstractions
{
    public interface IBackupManager
    {
        Task CreateBackupAsync(string source, string destination);
    }
}