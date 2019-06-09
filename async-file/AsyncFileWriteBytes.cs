using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
    public partial class AsyncFile
    {
        public Task WriteBytesAsync(string path, byte[] bytes)
        {
            return WriteBytesAsync(path, bytes, CancellationToken.None);
        }

        public Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
        {
            return WriteBytesAsync(path, bytes, 0, cancellationToken);
        }

        public Task WriteBytesAsync(string path, byte[] bytes, long offset)
        {
            return WriteBytesAsync(path, bytes, offset, CancellationToken.None);
        }

        public async Task WriteBytesAsync(string path, byte[] bytes, long offset, CancellationToken cancellationToken)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write, BufferSize,
                FileOptions))
            {
                stream.Position = offset;
                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}