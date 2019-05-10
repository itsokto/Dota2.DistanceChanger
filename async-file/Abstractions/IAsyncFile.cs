namespace Async.IO.Abstractions
{
    public interface IAsyncFile : IAsyncFileReadBytes, IAsyncFileWriteBytes, IAsyncFileReadAsString,
        IAsyncFileWriteString, IAsyncFileCopy
    {
    }
}