using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
    public partial class AsyncFile
    {
        public Task<byte[]> ReadBytesAsync(string path)
        {
            return ReadBytesAsync(path, CancellationToken.None);
        }

        public Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken)
        {
            return ReadBytesAsync(path, 0, 0, cancellationToken);
        }

        public Task<byte[]> ReadBytesAsync(string path, long offset)
        {
            return ReadBytesAsync(path, offset, 0, CancellationToken.None);
        }

        public Task<byte[]> ReadBytesAsync(string path, long offset, CancellationToken cancellationToken)
        {
            return ReadBytesAsync(path, offset, 0, cancellationToken);
        }

        public Task<byte[]> ReadBytesAsync(string path, long offset, long count)
        {
            return ReadBytesAsync(path, offset, count, CancellationToken.None);
        }

        public async Task<byte[]> ReadBytesAsync(string path, long offset, long count,
            CancellationToken cancellationToken)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize,
                FileOptions))
            {
                count = count == 0 ? stream.Length : count;

                var buffer = new byte[count];
                stream.Position = offset;
                await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)
                    .ConfigureAwait(false);
                return buffer;
            }
        }
    }
}