using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Patcher.Abstractions.Async;

namespace Dota2.DistanceChanger.Patcher
{
    // ReSharper disable once InconsistentNaming
    public class FileIO : IFileIO
    {
        private const FileOptions FileOptions =
            System.IO.FileOptions.Asynchronous | System.IO.FileOptions.SequentialScan;

        private readonly int _bufferSize = 4096;

        public FileIO(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        public FileIO()
        {
        }

        public async Task<byte[]> ReadBytesAsync(string path, long offset = 0L, long count = 0L,
            CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferSize,
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

        public async Task<string> ReadAsStringAsync(string path, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferSize,
                FileOptions))
            using (var reader = new StreamReader(stream))
            {
                var result = await reader.ReadToEndAsync()
                    .ConfigureAwait(false);
                return result;
            }
        }

        public async Task WriteStringAsync(string path, string content, CancellationToken cancellationToken = default)
        {
            var stream =
                new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, _bufferSize, FileOptions);

            using (var writer = new StreamWriter(stream))

            {
                await writer.WriteAsync(content)
                    .ConfigureAwait(false);
            }
        }

        public async Task WriteBytesAsync(string path, byte[] bytes, long offset = 0L,
            CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write,
                _bufferSize,
                FileOptions))
            {
                stream.Position = offset;
                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task CopyFileAsync(string sourceFile, string destinationFile, int bufferSize = 4096,
            CancellationToken cancellationToken = default)
        {
            using (var sourceStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferSize, FileOptions))

            using (var destinationStream =
                new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, _bufferSize,
                    FileOptions))

            {
                await sourceStream.CopyToAsync(destinationStream, bufferSize, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}