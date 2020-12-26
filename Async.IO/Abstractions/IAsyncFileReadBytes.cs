using System.Threading;
using System.Threading.Tasks;

namespace Async.IO.Abstractions
{
    public interface IAsyncFileReadBytes
    {
        Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken = default);

        Task<byte[]> ReadBytesAsync(string path, long offset, CancellationToken cancellationToken = default);

        Task<byte[]> ReadBytesAsync(string path, long offset, long count, CancellationToken cancellationToken = default);
    }
}