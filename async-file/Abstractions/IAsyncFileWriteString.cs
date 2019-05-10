using System.Threading;
using System.Threading.Tasks;

namespace Async.IO.Abstractions
{
    public interface IAsyncFileWriteString
    {
        Task WriteStringAsync(string path, string content);
        Task WriteStringAsync(string path, string content, CancellationToken cancellationToken);
    }
}