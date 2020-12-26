using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Abstractions
{
	public interface ISettingsManager<T>
	{
		Task<T> LoadAsync();

		Task<bool> SaveAsync(T settings);
	}
}