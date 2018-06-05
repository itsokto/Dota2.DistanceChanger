using System.Threading;
using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Patcher.Abstractions.Async
{
    // ReSharper disable once InconsistentNaming
    public interface IFileIO
    {
        Task<byte[]> ReadBytesAsync(string path, long offset = 0, long count = 0);
        Task<string> ReadAsStringAsync(string path);
        Task WriteAsync(string path, string content);
        Task WriteBytesAsync(string path, byte[] distance, long offset);

        Task CopyFileAsync(string sourceFile, string destinationFile,
            CancellationToken cancellationToken = default);
    }
}