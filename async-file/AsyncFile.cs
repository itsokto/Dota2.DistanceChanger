using System.IO;
using Async.IO.Abstractions;

namespace Async.IO
{
    public partial class AsyncFile : IAsyncFile
    {
        private const FileOptions FileOptions =
            System.IO.FileOptions.Asynchronous | System.IO.FileOptions.SequentialScan;

        private const int BufferSize = 4096;
    }
}