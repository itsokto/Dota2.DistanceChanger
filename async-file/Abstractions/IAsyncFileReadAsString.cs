using System.Threading;
using System.Threading.Tasks;

namespace Async.IO.Abstractions
{
    public interface IAsyncFileReadAsString
    {
        Task<string> ReadAsStringAsync(string path);

        Task<string> ReadAsStringAsync(string path, CancellationToken cancellationToken);
    }
}