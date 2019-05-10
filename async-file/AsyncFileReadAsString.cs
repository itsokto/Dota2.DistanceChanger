using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
    public partial class AsyncFile
    {
        public Task<string> ReadAsStringAsync(string path)
        {
            return ReadAsStringAsync(path, CancellationToken.None);
        }

        public async Task<string> ReadAsStringAsync(string path, CancellationToken cancellationToken)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions))
            using (var reader = new StreamReader(stream))
            {
                var result = await reader.ReadToEndAsync()
                    .ConfigureAwait(false);
                return result;
            }
        }
    }
}