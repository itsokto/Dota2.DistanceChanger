using System;
using Dota2.DistanceChanger.Core.Extensions;
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
			ReactiveMvvm.IoC.RegisterDotaClientDistance();

			ReactiveMvvm.IoC.RegisterPersistence();

			ReactiveMvvm.IoC.RegisterServices();

			ReactiveMvvm.IoC.RegisterInstance<IScreen>(this);

			ReactiveMvvm.IoC.RegisterViewModels(Reuse.Singleton);
		}
	}
}