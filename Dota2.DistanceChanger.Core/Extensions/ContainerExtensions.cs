using System.Linq;
using System.Reflection;
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

        public static void RegisterViews(this IContainer container, IReuse reuse)
        {
            var types = Assembly.GetCallingAssembly().GetTypes().Where(x => x.Name.EndsWith("View"));
            container.RegisterMany(types, reuse, serviceTypeCondition: type => type.Name.StartsWith(nameof(IViewFor)) && type.IsGenericType);
        }
    }
}