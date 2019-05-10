using System.Threading;
using System.Threading.Tasks;

namespace Async.IO.Abstractions
{
    public interface IAsyncFileCopy
    {
        Task CopyFileAsync(string sourceFile, string destinationFile);

        Task CopyFileAsync(string sourceFile, string destinationFile, CancellationToken cancellationToken);

        Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize);

        Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize,
            CancellationToken cancellationToken);
    }
}