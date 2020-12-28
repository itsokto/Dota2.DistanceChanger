using System.Windows;
using Dota2.DistanceChanger.Core;
using Dota2.DistanceChanger.Core.Extensions;
using Dota2.DistanceChanger.Core.Platforms.Shared;
using Dota2.DistanceChanger.Platform;
using DryIoc;

namespace Dota2.DistanceChanger
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			ReactiveMvvm.IoC.Register<IUserDialogs, UserDialog>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IUserInterface, UserInterface>(Reuse.Singleton);
			ReactiveMvvm.IoC.Register<IDotaLocation, DotaLocation>(Reuse.Singleton);

			ReactiveMvvm.IoC.RegisterViewsForViewModels(Reuse.Singleton);

			base.OnStartup(e);
		}
	}
}