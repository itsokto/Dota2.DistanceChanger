using Async.IO;
using Async.IO.Abstractions;
using Dota.Patcher.Core;
using Dota.Patcher.Core.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Infrastructure;
using Dota2.DistanceChanger.Core.Models;
using Dota2.DistanceChanger.Core.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Dota2.DistanceChanger.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<ISnackbarMessageQueue, SnackbarMessageQueue>();
            Mvx.IoCProvider.RegisterType<ISettingsManager<Settings>, SettingsManager>();
            Mvx.IoCProvider.RegisterType<IAsyncFile, AsyncFile>();
            Mvx.IoCProvider.RegisterType<IClientDistanceFinder, ClientDistanceFinder>();
            Mvx.IoCProvider.RegisterType<IBackupManager, BackupManager>();
            Mvx.IoCProvider.RegisterType<IDistancePatcher, DistancePatcher>();
            
            Mvx.IoCProvider.RegisterType(typeof(ILogger<>), typeof(Logger<>));
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ILoggerFactory, LoggerFactory>().AddFile("log.txt");

            RegisterAppStart<MainViewModel>();
        }
    }
}