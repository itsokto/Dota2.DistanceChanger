using System.Threading;
using System.Threading.Tasks;

namespace Async.IO.Abstractions
{
    public interface IAsyncFileReadBytes
    {
        Task<byte[]> ReadBytesAsync(string path);

        Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken);

        Task<byte[]> ReadBytesAsync(string path, long offset);

        Task<byte[]> ReadBytesAsync(string path, long offset, CancellationToken cancellationToken);

        Task<byte[]> ReadBytesAsync(string path, long offset, long count);

        Task<byte[]> ReadBytesAsync(string path, long offset, long count, CancellationToken cancellationToken);
    }
}