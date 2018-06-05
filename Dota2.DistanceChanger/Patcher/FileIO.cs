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

        private const int BufferSize = 4096;

        public async Task<byte[]> ReadBytesAsync(string path, long offset = 0, long count = 0)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize,
                FileOptions))
            {
                count = count == 0 ? stream.Length : count;

                var buffer = new byte[count];
                stream.Position = offset;
                await stream.ReadAsync(buffer, 0, buffer.Length)
                    .ConfigureAwait(false);
                return buffer;
            }
        }

        public async Task<string> ReadAsStringAsync(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize,
                FileOptions))
            using (var reader = new StreamReader(stream))
            {
                var result = await reader.ReadToEndAsync()
                    .ConfigureAwait(false);
                return result;
            }
        }

        public async Task WriteAsync(string path, string content)
        {
            var stream =
                new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, BufferSize, FileOptions);

            using (var writer = new StreamWriter(stream))

            {
                await writer.FlushAsync();
                await writer.WriteAsync(content)
                    .ConfigureAwait(false);
            }
        }

        public async Task WriteBytesAsync(string path, byte[] bytes, long offset = 0)
        {
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write,
                BufferSize,
                FileOptions))
            {
                stream.Position = offset;
                await stream.WriteAsync(bytes, 0, bytes.Length)
                    .ConfigureAwait(false);
            }
        }

        public async Task CopyFileAsync(string sourceFile, string destinationFile,
            CancellationToken cancellationToken = default)
        {
            using (var sourceStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions))

            using (var destinationStream =
                new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, BufferSize,
                    FileOptions))

            {
                await sourceStream.CopyToAsync(destinationStream, 81920, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}