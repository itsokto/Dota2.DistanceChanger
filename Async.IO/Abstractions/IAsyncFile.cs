namespace Async.IO.Abstractions
{
	public interface IAsyncFile : IAsyncFileReadBytes, IAsyncFileWriteBytes, IAsyncFileCopy
	{
		bool Exists(string path);
	}
}