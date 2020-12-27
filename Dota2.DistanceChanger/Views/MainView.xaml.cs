using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using MaterialDesignThemes.Wpf;
using ReactiveUI;

namespace Dota2.DistanceChanger.Views
{
	public partial class MainView
	{
		public MainView()
		{
			InitializeComponent();

			this.WhenActivated(disposable =>
			{
				ViewModel.WhenAnyValue(model => model.SettingsViewModel.Settings.Value.X32Client.DisplayName)
						 .Where(displayName => !string.IsNullOrWhiteSpace(displayName))
						 .Subscribe(displayName => HintAssist.SetHint(X32Client, displayName));

				ViewModel.WhenAnyValue(model => model.SettingsViewModel.Settings.Value.X64Client.DisplayName)
						 .Where(displayName => !string.IsNullOrWhiteSpace(displayName))
						 .Subscribe(displayName => HintAssist.SetHint(X64Client, displayName));

				this.Bind(ViewModel, model => model.SettingsViewModel.Settings.Value.X32Client.Distance.Value,
						  view => view.X32Client.Text)
					.DisposeWith(disposable);

				this.Bind(ViewModel, model => model.SettingsViewModel.Settings.Value.X64Client.Distance.Value,
						  view => view.X64Client.Text)
					.DisposeWith(disposable);

				this.Bind(ViewModel, model => model.SettingsViewModel.Settings.Value.Dota2FolderPath,
						  view => view.Dota2FolderPathTxtB.Text)
					.DisposeWith(disposable);

				this.Bind(ViewModel, model => model.SettingsViewModel.Settings.Value.DarkMode, view => view.DarkModeTgb.IsChecked)
					.DisposeWith(disposable);

				this.Bind(ViewModel, model => model.SettingsViewModel.Settings.Value.Backup, view => view.BackupChkb.IsChecked)
					.DisposeWith(disposable);

				this.BindCommand(ViewModel, model => model.PatchCommand, view => view.PatchButton,
								 model => model.SettingsViewModel.Settings.Value)
					.DisposeWith(disposable);

				this.BindCommand(ViewModel, model => model.SettingsViewModel.BrowseDota2FolderCommand, view => view.BrowseDota2FolderBtn)
					.DisposeWith(disposable);

                Window.GetWindow(this).Events().Closing
                    .Select(_ => ViewModel.SettingsViewModel.Settings.Value)
                    .InvokeCommand(ViewModel.SettingsViewModel.SaveSettingsCommand);

            });
		}
	}
}