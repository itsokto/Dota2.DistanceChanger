using System.Threading;
using System.Threading.Tasks;

namespace Async.IO.Abstractions
{
    public interface IAsyncFileWriteBytes
    {
        Task WriteBytesAsync(string path, byte[] bytes);

        Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken);

        Task WriteBytesAsync(string path, byte[] bytes, long offset);

        Task WriteBytesAsync(string path, byte[] bytes, long offset, CancellationToken cancellationToken);
    }
}