using System.Threading;
using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Patcher.Abstractions.Async
{
    // ReSharper disable once InconsistentNaming
    public interface IFileIO
    {
        Task<byte[]> ReadBytesAsync(string path, long offset = 0, long count = 0, CancellationToken cancellationToken = default);
        Task<string> ReadAsStringAsync(string path, CancellationToken cancellationToken = default);
        Task WriteStringAsync(string path, string content, CancellationToken cancellationToken = default);
        Task WriteBytesAsync(string path, byte[] distance, long offset = 0, CancellationToken cancellationToken = default);

        Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize = 4096,
            CancellationToken cancellationToken = default);
    }
}