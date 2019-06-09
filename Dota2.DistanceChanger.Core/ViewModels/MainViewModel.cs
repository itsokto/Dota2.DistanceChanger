using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dota.Patcher.Core.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Extensions;
using Dota2.DistanceChanger.Core.Models;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using PropertyChanged;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
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
        private readonly IUserDialogs _userDialogs;
        private readonly IUserInterface _userInterface;

        public MainViewModel(ILogger<MainViewModel> logger,
            IUserDialogs userDialogs,
            IUserInterface userInterface,
            IDistancePatcher distancePatcher,
            ISettingsManager<Settings> settingsManager,
            IBackupManager backupManager)
        {
            _logger = logger;
            _userDialogs = userDialogs;
            _userInterface = userInterface;
            _distancePatcher = distancePatcher;
            _settingsManager = settingsManager;
            _backupManager = backupManager;

            Settings = _settingsManager.LoadSettings()
                .SelectMany(async x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.Dota2FolderPath))
                        await x.Clients.ForEachAsync(async client =>
                        {
                            var fullPath = x.Dota2FolderPath + client.LocalPath;
                            var distance = await _distancePatcher.GetAsync(fullPath, x.Patterns);
                            client.Distance = distance.FirstOrDefault().Value;
                            client.CurrentDistance = client.Distance;
                        });

                    //TODO: handle case when Dota2FolderPath not found

                    return x;
                }).ToReactiveProperty();

            var canExecute =
                this.WhenAnyValue(x => x.Settings.Value.Dota2FolderPath, x => x.Settings.Value.Clients,
                        (path, clients) =>
                            !string.IsNullOrWhiteSpace(path)
                            && clients != null
                            && clients.All(x => !string.IsNullOrWhiteSpace(x.LocalPath)))
                    .ObserveOnUIDispatcher();

            PatchCommand = ReactiveCommand.CreateFromTask(CreatePatch, canExecute);

            PatchCommand.SubscribeOnUIDispatcher().Subscribe(_ => { _userDialogs.Alert("Done!"); });

            ToggleDarkModeCommand = ReactiveCommand.Create<bool>(ToggleDarkMode);

            Settings.Where(settings => settings != null)
                .Select(settings => settings.DarkMode)
                .ObserveOnUIDispatcher()
                .InvokeCommand(ToggleDarkModeCommand);

            Settings.Where(settings => settings != null)
                .Subscribe(settings => { settings.PropertyChanged += SettingsOnPropertyChanged; });
        }

        public ReactiveProperty<Settings> Settings { get; set; }

        public ReactiveCommand<Unit, Unit> PatchCommand { get; }

        public ReactiveCommand<bool, Unit> ToggleDarkModeCommand { get; }

        private void ToggleDarkMode(bool enable)
        {
            _userInterface.DarkMode(enable);
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _settingsManager.SaveSettings(Settings.Value).GetAwaiter().GetResult();
        }

        private Task CreatePatch()
        {
            return Settings.Value.Clients.ForEachAsync(async client =>
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
        }
    }
}