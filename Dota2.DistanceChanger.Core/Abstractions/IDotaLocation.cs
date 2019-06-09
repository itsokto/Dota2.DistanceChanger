using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Abstractions
{
    public interface IDotaLocation
    {
        Task<string> GetAsync();
    }
}