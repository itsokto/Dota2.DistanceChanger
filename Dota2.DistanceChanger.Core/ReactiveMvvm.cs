using DryIoc;
using Splat.DryIoc;

namespace Dota2.DistanceChanger.Core
{
	public static class ReactiveMvvm
	{
		static ReactiveMvvm()
		{
			IoC = new Container();
			IoC.UseDryIocDependencyResolver();
		}

		public static readonly IContainer IoC;
	}
}