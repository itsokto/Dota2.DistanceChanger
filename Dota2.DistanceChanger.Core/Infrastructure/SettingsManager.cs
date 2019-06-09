using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Async.IO.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Models;
using DynamicData.Binding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dota2.DistanceChanger.Core.Infrastructure
{
    public class SettingsManager : ISettingsManager<Settings>
    {
        private readonly IAsyncFile _asyncFile;
        private readonly IDotaLocation _dotaLocation;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

        public SettingsManager(IAsyncFile asyncFile, IDotaLocation dotaLocation)
        {
            _asyncFile = asyncFile;
            _dotaLocation = dotaLocation;
        }

        public string FilePath { get; internal set; } = "settings.json";

        public IObservable<Settings> LoadSettings()
        {
            return File.Exists(FilePath)
                ? _asyncFile.ReadAsStringAsync(FilePath)
                    .ToObservable()
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .SelectMany(async result =>
                    {
                        if (TryDeserializeObject<Settings>(result, out var settings))
                        {
                            if (string.IsNullOrWhiteSpace(settings.Dota2FolderPath))
                                settings.Dota2FolderPath = await _dotaLocation.GetAsync();

                            return settings;
                        }

                        return await CreateDefaultSettings();
                    })
                : CreateDefaultSettings().ToObservable();
        }

        public async Task<bool> SaveSettings(Settings settings)
        {
            var result = JsonConvert.SerializeObject(settings, _jsonSerializerSettings);
            await _asyncFile.WriteStringAsync(FilePath, result);
            return true;
        }

        private async Task<Settings> CreateDefaultSettings()
        {
            var clients = new ObservableCollectionExtended<Client>
            {
                new Client
                {
                    DisplayName = "X32 Client",
                    LocalPath = @"\game\dota\bin\win32\client.dll"
                },
                new Client
                {
                    DisplayName = "X64 Client",
                    LocalPath = @"\game\dota\bin\win64\client.dll"
                }
            };


            var patterns = new ObservableCollectionExtended<byte[]>
            {
                new byte[] //dota_camera_distance_min
                {
                    0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x64, 0x69, 0x73, 0x74,
                    0x61, 0x6E, 0x63, 0x65, 0x5F, 0x6D, 0x69, 0x6E, 0x00
                },
                new byte[] //dota_camera_distance
                {
                    0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x64, 0x69, 0x73, 0x74,
                    0x61, 0x6E, 0x63, 0x65, 0x00, 0x00
                },
                new byte[] // dota_camera_movement_frametime_multiplier
                {
                    0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x6D, 0x6F, 0x76, 0x65,
                    0x6D, 0x65, 0x6E, 0x74, 0x5F, 0x66, 0x72, 0x61, 0x6D, 0x65, 0x74, 0x69, 0x6D, 0x65, 0x5F, 0x6D,
                    0x75, 0x6C, 0x74, 0x69, 0x70, 0x6C, 0x69, 0x65, 0x72
                },
                new byte[] //dota_camera_pitch_max
                {
                    0x00, 0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x70, 0x69, 0x74,
                    0x63, 0x68, 0x5F, 0x6D, 0x61, 0x78, 0x00
                }
            };

            var settingsObj = new Settings
            {
                Backup = true,
                Clients = clients,
                DarkMode = true,
                Dota2FolderPath = await _dotaLocation.GetAsync(),
                Patterns = patterns
            };

            var resultStr = JsonConvert.SerializeObject(settingsObj, _jsonSerializerSettings);
            await _asyncFile.WriteStringAsync(FilePath, resultStr);
            return settingsObj;
        }

        private bool TryDeserializeObject<T>(string value, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(value, _jsonSerializerSettings);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}