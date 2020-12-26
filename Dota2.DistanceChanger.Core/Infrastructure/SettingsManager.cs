using System.IO.Abstractions;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Models;
using Dota2.DistanceChanger.Core.Platforms.Shared;
using DynamicData.Binding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dota2.DistanceChanger.Core.Infrastructure
{
	public class SettingsManager : ISettingsManager<Settings>
	{
		private readonly IFileSystem _fileSystem;

		private readonly IDotaLocation _dotaLocation;

		private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
		{
			ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
			Formatting = Formatting.Indented
		};

		public SettingsManager(IFileSystem fileSystem, IDotaLocation dotaLocation)
		{
			_fileSystem = fileSystem;
			_dotaLocation = dotaLocation;
		}

		public string FilePath { get; private set; } = "settings.json";

		public async Task<Settings> LoadAsync()
		{
			Settings settings;

			if (!_fileSystem.File.Exists(FilePath))
			{
				settings = await CreateDefaultSettings();
			}
			else
			{
				var fileString = await _fileSystem.File.ReadAllTextAsync(FilePath).ConfigureAwait(false);

				if (!TryDeserialize(fileString, out settings))
				{
					settings = await CreateDefaultSettings();
				}
			}

			return settings;
		}

		public async Task<bool> SaveAsync(Settings settings)
		{
			var result = JsonConvert.SerializeObject(settings, _jsonSerializerSettings);

			await _fileSystem.File.WriteAllTextAsync(FilePath, result).ConfigureAwait(false);

			return true;
		}

		private async Task<Settings> CreateDefaultSettings()
		{
			var x32Client = new Client
			{
				DisplayName = "X32 Client",
				LocalPath = @"\game\dota\bin\win32\client.dll"
			};

			var x64Client = new Client
			{
				DisplayName = "X64 Client",
				LocalPath = @"\game\dota\bin\win64\client.dll"
			};

			var patterns = new ObservableCollectionExtended<byte[]>
			{
				new byte[] //dota_camera_distance_min
				{
					0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x64, 0x69, 0x73, 0x74, 0x61, 0x6E, 0x63, 0x65, 0x5F, 0x6D,
					0x69, 0x6E, 0x00
				},
				new byte[] //dota_camera_distance
				{
					0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x64, 0x69, 0x73, 0x74, 0x61, 0x6E, 0x63, 0x65, 0x00, 0x00
				},
				new byte[] // dota_camera_movement_frametime_multiplier
				{
					0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x6D, 0x6F, 0x76, 0x65, 0x6D, 0x65, 0x6E, 0x74, 0x5F, 0x66,
					0x72, 0x61, 0x6D, 0x65, 0x74, 0x69, 0x6D, 0x65, 0x5F, 0x6D, 0x75, 0x6C, 0x74, 0x69, 0x70, 0x6C, 0x69, 0x65, 0x72
				},
				new byte[] //dota_camera_pitch_max
				{
					0x00, 0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x70, 0x69, 0x74, 0x63, 0x68, 0x5F, 0x6D, 0x61, 0x78,
					0x00
				}
			};

			var dotaFolder = await _dotaLocation.GetAsync().ConfigureAwait(false);

			var settingsObj = new Settings
			{
				Backup = true,
				Dota2FolderPath = dotaFolder,
				X32Client = x32Client,
				X64Client = x64Client,
				DarkMode = true,
				Patterns = patterns
			};

			return settingsObj;
		}

		private bool TryDeserialize<T>(string value, out T result)
		{
			result = default;

			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}

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