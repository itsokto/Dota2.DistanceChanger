using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Async.IO
{
	public partial class AsyncFile
	{
		public Task CopyFileAsync(string sourceFile, string destinationFile, CancellationToken cancellationToken)
		{
			return CopyFileAsync(sourceFile, destinationFile, DefaultBufferSize, CancellationToken.None);
		}

		public async Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize, CancellationToken cancellationToken)
		{
			await using var sourceStream = AsyncStreamReader(sourceFile, Encoding.UTF8).BaseStream;
			await using var destinationStream = AsyncStreamWriter(destinationFile, Encoding.UTF8, false).BaseStream;
			await sourceStream.CopyToAsync(destinationStream, bufferSize, cancellationToken).ConfigureAwait(false);
		}
	}
}