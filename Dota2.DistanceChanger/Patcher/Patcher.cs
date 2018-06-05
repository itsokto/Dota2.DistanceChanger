using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Patcher.Abstractions;
using Dota2.DistanceChanger.Patcher.Abstractions.Async;

namespace Dota2.DistanceChanger.Patcher
{
    internal class Patcher : IPatcher
    {
        private readonly IBackupManager _backupManager;
        private readonly IClientDistanceFinder _clientDistanceFinder;

        // ReSharper disable once InconsistentNaming
        private readonly IFileIO _fileIO;

        // ReSharper disable once InconsistentNaming
        public Patcher(IFileIO fileIO, IClientDistanceFinder clientDistanceFinder,
            IBackupManager backupManager)
        {
            _fileIO = fileIO;
            _clientDistanceFinder = clientDistanceFinder;
            _backupManager = backupManager;
        }

        public async Task SetDistanceAsync(string path, string distance, IEnumerable<byte[]> patterns)
        {
            var indices = await GetDistanceAsync(path, patterns);

            await SetDistanceAsync(path, distance, indices.Keys);
        }

        public async Task SetDistanceAsync(string path, string distance, IEnumerable<long> offsets)
        {
            var encodedDistance = Encoding.Default.GetBytes(distance);
            foreach (var offset in offsets)
                await _fileIO.WriteBytesAsync(path, encodedDistance, offset);
        }

        public Task SetDistanceAsync(string path, string distance, long offset)
        {
            return SetDistanceAsync(path, distance, new[] {offset});
        }

        public async Task<IDictionary<long, string>> GetDistanceAsync(string path, IEnumerable<byte[]> patterns)
        {
            var buffer = await _fileIO.ReadBytesAsync(path);
            var dictionary = _clientDistanceFinder.Get(buffer, patterns);
            return dictionary;
        }

        public async Task<IDictionary<long, string>> GetDistanceAsync(string path, IEnumerable<long> offsets)
        {
            var buffer = await _fileIO.ReadBytesAsync(path);
            var dictionary = _clientDistanceFinder.Get(buffer, offsets);
            return dictionary;
        }

        public async Task<string> GetDistanceAsync(string path, long offset)
        {
            var buffer = await _fileIO.ReadBytesAsync(path);
            return _clientDistanceFinder.Get(buffer, offset);
        }

        public async Task CreateBackupAsync(string source, string destination)
        {
            await _backupManager.CreateBackupAsync(source, destination);
        }
    }
}