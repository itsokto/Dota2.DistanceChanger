using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using Async.IO;
using Async.IO.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Infrastructure;
using Dota2.DistanceChanger.Core.Models;
using Dota2.Patcher.Core;
using Dota2.Patcher.Core.Abstractions;
using DryIoc;
using ReactiveUI;

namespace Dota2.DistanceChanger.Core.Extensions
{
    public static class ContainerExtensions
    {
        public static void RegisterViewModels(this IContainer container, IReuse reuse)
        {
            var types = Assembly.GetCallingAssembly().GetTypes().Where(x => x.Name.EndsWith("ViewModel"));
            container.RegisterMany(types, reuse);
        }

        public static void RegisterViewsForViewModels(this IContainer container, IReuse reuse)
        {
            var types = Assembly.GetCallingAssembly().GetTypes().Where(x => x.Name.EndsWith("View"));
            container.RegisterMany(types, reuse, serviceTypeCondition: type => type.Name.StartsWith(nameof(IViewFor)) && type.IsGenericType);
        }
        
        public static void RegisterPersistence(this IContainer container)
        {
            container.Register<IFileSystem, FileSystem>(Reuse.Singleton);
            container.Register<IAsyncFile, AsyncFile>(Reuse.Singleton);

            container.Register<IBackupManager, BackupManager>(Reuse.Singleton);
            container.Register<ISettingsManager<Settings>, SettingsManager>(Reuse.Singleton);
        }
        
        public static void RegisterServices(this IContainer container)
        {
            ReactiveMvvm.IoC.Register<IDotaClientDistanceLoader, DotaClientDistanceLoader>(Reuse.Singleton);
            ReactiveMvvm.IoC.Register<IDotaClientDistancePatcher, DotaClientDistancePatcher>(Reuse.Singleton);
        }
        
        public static void RegisterDotaClientDistance(this IContainer container)
        {
            ReactiveMvvm.IoC.Register<IDotaClientDistanceParser, DotaClientDistanceParser>(Reuse.Singleton);
            ReactiveMvvm.IoC.Register<IDotaClientDistance, DotaClientDistance>(Reuse.Singleton);
        }
    }
}