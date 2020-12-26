using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Models;
using Dota2.DistanceChanger.Core.Platforms.Shared;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Dota2.DistanceChanger.Core.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly IUserDialogs _userDialogs;

        public SettingsViewModel(IUserInterface userInterface,
            IUserDialogs userDialogs,
            IDotaClientDistanceLoader distanceLoader,
            ISettingsManager<Settings> settingsManager)
        {
            _userDialogs = userDialogs;
            Settings = settingsManager.LoadAsync().ToObservable().ToReactiveProperty();

            SaveSettingsCommand = ReactiveCommand.CreateFromTask<Settings>(settingsManager.SaveAsync);

            ToggleDarkModeCommand = ReactiveCommand.Create<bool>(userInterface.DarkMode);

            LoadDistanceCommand = ReactiveCommand.CreateFromTask<Settings>(distanceLoader.LoadAsync);

            BrowseDota2FolderCommand = ReactiveCommand.CreateFromTask(OpenFolderDialog);

            BrowseDota2FolderCommand.Where(result => !string.IsNullOrWhiteSpace(result))
                .Select(_ => Settings.Value)
                .InvokeCommand(LoadDistanceCommand);

            BrowseDota2FolderCommand.SubscribeOnUIDispatcher().Subscribe(result =>
            {
                if (string.IsNullOrWhiteSpace(result))
                {
                    userDialogs.Alert("Incorrect path!");
                    return;
                }

                Settings.Value.Dota2FolderPath = result;
            });

            this.WhenAnyValue(vm => vm.Settings.Value.DarkMode)
                .InvokeCommand(ToggleDarkModeCommand);
        }

        public ReactiveProperty<Settings> Settings { get; }

        public ReactiveCommand<bool, Unit> ToggleDarkModeCommand { get; }

        public ReactiveCommand<Unit, string> BrowseDota2FolderCommand { get; }

        public ReactiveCommand<Settings, Unit> SaveSettingsCommand { get; }

        private ReactiveCommand<Settings, Unit> LoadDistanceCommand { get; }

        private async Task<string> OpenFolderDialog()
        {
            var dotaFolder = await _userDialogs.OpenFolderDialog();

            if (string.IsNullOrWhiteSpace(dotaFolder))
            {
                return string.Empty;
            }

            var directoryInfo = new DirectoryInfo(dotaFolder);

            return directoryInfo.Name == "dota 2 beta" ? dotaFolder : string.Empty;
        }
    }
}