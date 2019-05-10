using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dota.Patcher.Core.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using PropertyChanged;
using Reactive.Bindings;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Dota2.DistanceChanger.Core.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : MvxViewModel
    {
        private readonly IBackupManager _backupManager;
        private readonly IDistancePatcher _distancePatcher;
        private readonly ILogger<MainViewModel> _logger;
        private readonly ISettingsManager<Settings> _settingsManager;

        public MainViewModel(ILogger<MainViewModel> logger,
            ISnackbarMessageQueue snackbarMessageQueue,
            IDistancePatcher distancePatcher,
            ISettingsManager<Settings> settingsManager,
            IBackupManager backupManager)
        {
            _logger = logger;
            _distancePatcher = distancePatcher;
            SnackbarMessageQueue = snackbarMessageQueue;
            _settingsManager = settingsManager;
            _backupManager = backupManager;

            Settings = _settingsManager.LoadSettings()
                .SelectMany(async settings =>
                {
                    if (!string.IsNullOrWhiteSpace(settings.Dota2FolderPath))
                        foreach (var client in settings.Clients)
                        {
                            var fullPath = settings.Dota2FolderPath + client.LocalPath;
                            var distance = await _distancePatcher.GetAsync(fullPath, settings.Patterns);
                            client.Distance = distance.FirstOrDefault().Value;
                            client.CurrentDistance = client.Distance;
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

            PatchCommand.Subscribe(async unit =>
            {
                await _settingsManager.SaveSettings(Settings.Value);
                SnackbarMessageQueue.Enqueue("Done!");
            });

            ToggleDarkModeCommand = ReactiveCommand.CreateFromTask<bool>(async x =>
            {
                new PaletteHelper().SetLightDark(x);
                await _settingsManager.SaveSettings(Settings.Value);
            });

            Settings.Where(settings => settings != null)
                .Take(1)
                .Select(settings => settings.DarkMode)
                .InvokeCommand(ToggleDarkModeCommand);
        }

        public ReactiveProperty<Settings> Settings { get; set; }

        public ReactiveCommand<Unit, Unit> PatchCommand { get; }

        public ReactiveCommand<bool, Unit> ToggleDarkModeCommand { get; }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        private async Task CreatePatch()
        {
            var tasks = new List<Task>();

            foreach (var client in Settings.Value.Clients)
            {
                var task = Task.Run(async () =>
                {
                    var fullPath = Settings.Value.Dota2FolderPath + client.LocalPath;
                    if (Settings.Value.Backup)
                    {
                        _logger?.LogInformation($"Backing up {client.DisplayName}.");
                        await _backupManager.CreateBackupAsync(fullPath,
                            $"{fullPath}.back{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
                    }

                    if (client.CurrentDistance != client.Distance)
                    {
                        _logger?.LogInformation($"Patching {client.DisplayName}, distance {client.Distance}.");
                        await _distancePatcher.SetAsync(fullPath, client.Distance, Settings.Value.Patterns);

                        client.CurrentDistance = client.Distance;
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
    }
}