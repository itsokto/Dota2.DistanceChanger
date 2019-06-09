using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Infrastructure;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Core;

namespace Dota2.DistanceChanger
{
    public class AppSetup : MvxWpfSetup<Core.App>
    {
        protected override void InitializePlatformServices()
        {
            Mvx.IoCProvider.RegisterType<IUserDialogs, UserDialog>();
            Mvx.IoCProvider.RegisterType<IUserInterface, UserInterface>();
            Mvx.IoCProvider.RegisterType<IDotaLocation, DotaLocation>();
        }
    }
}