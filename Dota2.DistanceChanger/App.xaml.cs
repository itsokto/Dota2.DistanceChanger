﻿using System.Windows;
using Dota2.DistanceChanger.Core;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.ViewModels;
using Dota2.DistanceChanger.Platform;
using Dota2.DistanceChanger.Views;
using DryIoc;
using ReactiveUI;

namespace Dota2.DistanceChanger
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			ReactiveMvvm.IoC.Register<IUserDialogs, UserDialog>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IUserInterface, UserInterface>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IDotaLocation, DotaLocation>(Reuse.Singleton);

			ReactiveMvvm.IoC.Register<IViewFor<MainViewViewModel>, MainView>();
			ReactiveMvvm.IoC.Register<IViewFor<AppSetup>, RootWindow>();

			base.OnStartup(e);
		}
	}
}