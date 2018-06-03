using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamDist.Patcher.Abstractions;
using NLog;

namespace CamDist.Patcher
{
    internal class Patcher : IPatcher
    {
        private readonly IClientReader _clientReader;
        private readonly IClientWriter _clientWriter;
        private readonly IClientDistanceFinder _clientDistanceFinder;
        private readonly IBackupManager _backupManager;
        private readonly ILogger _logger;

        public Patcher(ILogger logger, IClientReader clientReader,
            IClientWriter clientWriter, IClientDistanceFinder clientDistanceFinder, IBackupManager backupManager)
        {
            _logger = logger;
            _clientReader = clientReader;
            _clientWriter = clientWriter;
            _clientDistanceFinder = clientDistanceFinder;
            _backupManager = backupManager;
        }

        public bool SetCustomDistance(string path, string distance, IEnumerable<byte[]> patterns)
        {
            _logger?.Debug($"Create patch for {path}, distance {distance}.");
            var dictionary = GetCurrentDistance(path, patterns);
            var encodedDistance = Encoding.Default.GetBytes(distance);
            foreach (var dic in dictionary) _clientWriter.Write(path, encodedDistance, dic.Key);
            var result = dictionary.Any();
            _logger?.Warn($"Patch result is {result}.");
            return result;
        }

        public IDictionary<long, string> GetCurrentDistance(string path, IEnumerable<byte[]> patterns)
        {
            var buffer = _clientReader.ReadBytes(path);
            var dictionary = _clientDistanceFinder.Find(buffer, patterns);

            _logger?.Debug($"Found indices: {Utils.PrintDictionary(dictionary)}");

            return dictionary;
        }

        public void CreateBackup(string source, string destdestination, bool isOverride)
        {
            _logger?.Debug($"Create backup for {source}, override is {isOverride}.");
            _backupManager.CreateBackupAsync(source, destdestination, isOverride);
        }
    }
}