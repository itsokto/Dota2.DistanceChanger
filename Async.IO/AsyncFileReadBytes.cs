using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
	public partial class AsyncFile
	{
		public Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken)
		{
			return ReadBytesAsync(path, 0, 0, cancellationToken);
		}

		public Task<byte[]> ReadBytesAsync(string path, long offset, CancellationToken cancellationToken)
		{
			return ReadBytesAsync(path, offset, 0, cancellationToken);
		}

		public async Task<byte[]> ReadBytesAsync(string path, long offset, long count, CancellationToken cancellationToken)
		{
			await using var stream = AsyncStreamForReading(path);

			count = count == 0
				? stream.Length
				: count;

			var buffer = new byte[count];

			stream.Seek(offset, SeekOrigin.Begin);

			await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);

			return buffer;
		}
	}
}