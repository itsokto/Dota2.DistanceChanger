using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Platforms.Shared
{
	public interface IDotaLocation
	{
		ValueTask<string> GetAsync();
	}
}