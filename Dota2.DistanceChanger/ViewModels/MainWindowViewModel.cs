using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Abstractions;
using Dota2.DistanceChanger.Infrastructure;
using Dota2.DistanceChanger.Models;
using Dota2.DistanceChanger.Patcher;
using Dota2.DistanceChanger.Patcher.Abstractions;
using Microsoft.Win32;
using Nito.AsyncEx;
using NLog;
using PropertyChanged;
using ReactiveUI;

namespace Dota2.DistanceChanger.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IPatcher _patcher;
        private readonly ISettingsManager _settingsManager;

        private readonly ObservableAsPropertyHelper<Settings> _settingsPropertyHelper;

        private readonly INotifyTaskCompletion<Settings> _settings;

        public MainWindowViewModel()
        {
            // ReSharper disable once InconsistentNaming
            var fileIO = new FileIO();

            _settingsManager = new SettingsManager(fileIO)
            {
                Path = $"{AppDomain.CurrentDomain.BaseDirectory}settings.json"
            };

            _patcher = new Patcher.Patcher(fileIO, new ClientDistanceFinder(), new BackupManager(fileIO));

            _settings = NotifyTaskCompletion.Create(async () =>
            {
                var setting = await _settingsManager.LoadSettings();
                if (string.IsNullOrWhiteSpace(setting.Dota2FolderPath))
                    setting.Dota2FolderPath = GetDotaInstallLocation();

                return setting;
            });

            _settingsPropertyHelper = this.WhenAnyValue(x => x._settings.Result)
                .Where(x => x != null)
                .ToProperty(this, x => x.Settings);

            var canExecute =
                this.WhenAnyValue(x => x.Settings.Dota2FolderPath, x => x.Settings.Clients,
                    (dota, clients) =>
                        !string.IsNullOrWhiteSpace(dota) && clients.All(x => !string.IsNullOrWhiteSpace(x.LocalPath)));

            canExecute.DistinctUntilChanged().Subscribe(async x =>
            {
                foreach (var client in _settingsPropertyHelper.Value.Clients)
                {
                    var fullPath = Settings.Dota2FolderPath + client.LocalPath;
                    var distance = await _patcher.GetDistanceAsync(fullPath, Settings.Patterns);
                    client.Distance = distance.FirstOrDefault().Value;
                }
            });

            PatchCommand = ReactiveCommand.CreateFromTask(CreatePatch, canExecute);
        }

        public Settings Settings => _settingsPropertyHelper.Value;
        public ReactiveCommand<Unit, Unit> PatchCommand { get; }

        private async Task CreatePatch()
        {
            foreach (var client in _settingsPropertyHelper.Value.Clients)
            {
                var fullPath = Settings.Dota2FolderPath + client.LocalPath;
                if (client.Backup)
                {
                    _logger?.Debug($"Create backup for {client.DisplayName}.");
                    await _patcher.CreateBackupAsync(fullPath,
                        $"{fullPath}.back{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
                }

                if (client.LastDistance != client.Distance)
                {
                    _logger?.Debug($"Create patch for {client.DisplayName}, distance {client.Distance}.");
                    await _patcher.SetDistanceAsync(fullPath, client.Distance, Settings.Patterns);

                    client.LastDistance = client.Distance;
                }
            }

            await _settingsManager.SaveSettings(Settings);
        }

        public string GetDotaInstallLocation()
        {
            var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 570");

            var value = key?.GetValue("InstallLocation");
            return value?.ToString();
        }
    }
}