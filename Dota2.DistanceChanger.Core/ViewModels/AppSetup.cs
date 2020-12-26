using System;
using System.IO.Abstractions;
using Async.IO;
using Async.IO.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Infrastructure;
using Dota2.DistanceChanger.Core.Models;
using Dota2.Patcher.Core;
using Dota2.Patcher.Core.Abstractions;
using DryIoc;
using ReactiveUI;

namespace Dota2.DistanceChanger.Core.ViewModels
{
	public class AppSetup : ReactiveObject, IScreen
	{
		public RoutingState Router { get; }

		public AppSetup()
		{
			Router = new RoutingState();

			RegisterDependencies();

			Router.Navigate.Execute(ReactiveMvvm.IoC.Resolve<MainViewViewModel>()).Subscribe();
		}

		private void RegisterDependencies()
		{
			ReactiveMvvm.IoC.Register<IFileSystem, FileSystem>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IAsyncFile, AsyncFile>(Reuse.Singleton);
			
			ReactiveMvvm.IoC.Register<IBackupManager, BackupManager>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<ISettingsManager<Settings>, SettingsManager>(Reuse.Singleton);
			
			ReactiveMvvm.IoC.Register<IDotaClientDistanceParser, DotaClientDistanceParser>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IDotaClientDistanceLoader, DotaClientDistanceLoader>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IDotaClientDistancePatcher, DotaClientDistancePatcher>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IDotaClientDistance, DotaClientDistance>(Reuse.Singleton);

			ReactiveMvvm.IoC.RegisterInstance<IScreen>(this);
			ReactiveMvvm.IoC.Register<MainViewViewModel>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<SettingsViewModel>(Reuse.Singleton);
		}
	}
}