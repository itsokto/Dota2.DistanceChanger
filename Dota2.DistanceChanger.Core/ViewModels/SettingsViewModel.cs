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

            BrowseDota2FolderCommand.Where(value => !string.IsNullOrWhiteSpace(value))
                .SubscribeOnUIDispatcher()
                .Subscribe(value => Settings.Value.Dota2FolderPath = value);

            this.WhenAnyValue(vm => vm.Settings.Value.DarkMode)
                .InvokeCommand(ToggleDarkModeCommand);

            this.WhenAnyValue(vm => vm.Settings.Value.Dota2FolderPath)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(_ => Settings.Value)
                .ObserveOnUIDispatcher()
                .InvokeCommand(LoadDistanceCommand);
        }

        public ReactiveProperty<Settings> Settings { get; }

        public ReactiveCommand<Unit, string> BrowseDota2FolderCommand { get; }

        public ReactiveCommand<Settings, Unit> SaveSettingsCommand { get; }

        private ReactiveCommand<Settings, Unit> LoadDistanceCommand { get; }

        private ReactiveCommand<bool, Unit> ToggleDarkModeCommand { get; }


        private async Task<string> OpenFolderDialog()
        {
            var dotaFolder = await _userDialogs.OpenFolderDialog();

            if (string.IsNullOrWhiteSpace(dotaFolder))
            {
                return string.Empty;
            }

            var directoryInfo = new DirectoryInfo(dotaFolder);

            return directoryInfo.Name == "dota 2 beta" ? directoryInfo.FullName : string.Empty;
        }
    }
}