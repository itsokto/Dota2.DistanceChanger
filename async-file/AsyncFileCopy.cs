using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
    public partial class AsyncFile
    {
        public Task CopyFileAsync(string sourceFile, string destinationFile)
        {
            return CopyFileAsync(sourceFile, destinationFile, CancellationToken.None);
        }

        public Task CopyFileAsync(string sourceFile, string destinationFile, CancellationToken cancellationToken)
        {
            return CopyFileAsync(sourceFile, destinationFile, 81920, CancellationToken.None);
        }

        public Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize)
        {
            return CopyFileAsync(sourceFile, destinationFile, bufferSize, CancellationToken.None);
        }

        public async Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize,
            CancellationToken cancellationToken)
        {
            using (var sourceStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions))

            using (var destinationStream =
                new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, BufferSize,
                    FileOptions))

            {
                await sourceStream.CopyToAsync(destinationStream, bufferSize, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}