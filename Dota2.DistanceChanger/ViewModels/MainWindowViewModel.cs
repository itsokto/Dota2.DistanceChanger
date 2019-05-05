using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Abstractions;
using Dota2.DistanceChanger.Infrastructure;
using Dota2.DistanceChanger.Models;
using Dota2.DistanceChanger.Patcher;
using Dota2.DistanceChanger.Patcher.Abstractions;
using MaterialDesignThemes.Wpf;
using NLog;
using PropertyChanged;
using Reactive.Bindings;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Dota2.DistanceChanger.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IPatcher _patcher;
        private readonly ISettingsManager _settingsManager;

        public MainWindowViewModel()
        {
            // ReSharper disable once InconsistentNaming
            var fileIO = new FileIO();

            _settingsManager = new SettingsManager(fileIO)
            {
                Path = "settings.json"
            };

            _patcher = new Patcher.Patcher(fileIO, new ClientDistanceFinder(), new BackupManager(fileIO));

            Settings = _settingsManager.LoadSettings()
                .SelectMany(async settings =>
                {
                    if (!string.IsNullOrWhiteSpace(settings.Dota2FolderPath))
                    {
                        foreach (var client in settings.Clients)
                        {
                            var fullPath = settings.Dota2FolderPath + client.LocalPath;
                            var distance = await _patcher.GetDistanceAsync(fullPath, settings.Patterns);
                            client.Distance = distance.FirstOrDefault().Value;
                        }
                    }

                    return settings;
                }).ToReactiveProperty();

            var canExecute =
                this.WhenAnyValue(x => x.Settings.Value.Dota2FolderPath, x => x.Settings.Value.Clients,
                        (path, clients) =>
                            !string.IsNullOrWhiteSpace(path) &&
                            clients.All(x => !string.IsNullOrWhiteSpace(x.LocalPath)))
                    .ObserveOn(RxApp.MainThreadScheduler);

            PatchCommand = ReactiveCommand.CreateFromTask(CreatePatch, canExecute);

            ToggleDarkModeCommand = ReactiveCommand.Create<bool>(x => new PaletteHelper().SetLightDark(x));
        }

        public ReactiveProperty<Settings> Settings { get; set; }

        public ReactiveCommand<Unit, Unit> PatchCommand { get; }

        public ReactiveCommand<bool, Unit> ToggleDarkModeCommand { get; }

        private async Task CreatePatch()
        {
            var tasks = new List<Task>();

            foreach (var client in Settings.Value.Clients)
            {
                var task = Task.Run(async () =>
                {
                    var fullPath = Settings.Value.Dota2FolderPath + client.LocalPath;
                    if (client.Backup)
                    {
                        _logger?.Debug($"Backing up {client.DisplayName}.");
                        await _patcher.CreateBackupAsync(fullPath,
                            $"{fullPath}.back{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
                    }

                    if (client.LastDistance != client.Distance)
                    {
                        _logger?.Debug($"Patching {client.DisplayName}, distance {client.Distance}.");
                        await _patcher.SetDistanceAsync(fullPath, client.Distance, Settings.Value.Patterns);

                        client.LastDistance = client.Distance;
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await _settingsManager.SaveSettings(Settings.Value);
        }
    }
}