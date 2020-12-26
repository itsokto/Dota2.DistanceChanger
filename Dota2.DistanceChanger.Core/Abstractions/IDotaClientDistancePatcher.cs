using System.Threading.Tasks;
using Dota2.DistanceChanger.Core.Models;

namespace Dota2.DistanceChanger.Core.Abstractions
{
	public interface IDotaClientDistancePatcher
	{
		Task PatchAsync(Settings settings);
	}
}