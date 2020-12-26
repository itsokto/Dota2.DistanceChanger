using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Models;
using Dota2.DistanceChanger.Core.Platforms.Shared;
using Reactive.Bindings.Extensions;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Dota2.DistanceChanger.Core.ViewModels
{
    public class MainViewViewModel : ReactiveObject, IRoutableViewModel
    {
        public MainViewViewModel(IUserDialogs userDialogs,
            IDotaClientDistancePatcher distancePatcher,
            SettingsViewModel settingsViewModel,
            IScreen hostScreen)
        {
            SettingsViewModel = settingsViewModel;
            HostScreen = hostScreen;

            var pathCanExecute = this.WhenAnyValue(
                    model => model.SettingsViewModel.Settings.Value.X32Client.Distance.Value,
                    model => model.SettingsViewModel.Settings.Value.X64Client.Distance.Value,
                    (x32, x64) => new[] {x32, x64})
                .Select(items => items.All(value => value >= 1000 && value <= 9999));

            PatchCommand = ReactiveCommand.CreateFromTask<Settings>(distancePatcher.PatchAsync, pathCanExecute);

            PatchCommand
                .SubscribeOnUIDispatcher()
                .Subscribe(_ => { userDialogs.Alert("Done!"); });

            PatchCommand.ThrownExceptions
                .SubscribeOnUIDispatcher()
                .Subscribe(exception => userDialogs.Alert("Error occurred!"));
        }

        public SettingsViewModel SettingsViewModel { get; }

        public ReactiveCommand<Settings, Unit> PatchCommand { get; }

        #region Navigation

        public string UrlPathSegment => "main";

        public IScreen HostScreen { get; }

        #endregion
    }
}