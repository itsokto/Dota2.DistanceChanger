using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
	public partial class AsyncFile
	{
		public Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
		{
			return WriteBytesAsync(path, bytes, 0, cancellationToken);
		}

		public async Task WriteBytesAsync(string path, byte[] bytes, long offset, CancellationToken cancellationToken)
		{
			await using var stream = AsyncStreamForWriting(path, false);

			stream.Seek(offset, SeekOrigin.Begin);

			await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
		}
	}
}