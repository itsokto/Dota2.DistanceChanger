using System.IO;
using System.IO.Abstractions;
using System.Text;
using Async.IO.Abstractions;

namespace Async.IO
{
	public partial class AsyncFile : IAsyncFile
	{
		private readonly IFileSystem _fileSystem;

		private const int DefaultBufferSize = 4096;

		public AsyncFile(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		private Stream AsyncStreamForReading(string path)
		{
			return _fileSystem.FileStream.Create(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize,
				FileOptions.Asynchronous | FileOptions.SequentialScan);
		}

		private Stream AsyncStreamForWriting(string path, bool append)
		{
			return _fileSystem.FileStream.Create(path, append
					? FileMode.Append
					: FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, DefaultBufferSize,
				FileOptions.Asynchronous | FileOptions.SequentialScan);
		}

		private StreamReader AsyncStreamReader(string path, Encoding encoding)
		{
			var stream = AsyncStreamForReading(path);

			return new StreamReader(stream, encoding, true);
		}

		private StreamWriter AsyncStreamWriter(string path, Encoding encoding, bool append)
		{
			var stream = AsyncStreamForWriting(path, append);

			return new StreamWriter(stream, encoding);
		}

		public bool Exists(string path)
		{
			return _fileSystem.File.Exists(path);
		}
	}
}