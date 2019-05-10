using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
    public partial class AsyncFile
    {
        public Task WriteStringAsync(string path, string content)
        {
            return WriteStringAsync(path, content, CancellationToken.None);
        }

        public async Task WriteStringAsync(string path, string content, CancellationToken cancellationToken)
        {
            var stream =
                new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, BufferSize, FileOptions);

            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(content)
                    .ConfigureAwait(false);
            }
        }
    }
}