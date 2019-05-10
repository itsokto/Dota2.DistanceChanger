using System;
using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Abstractions
{
    public interface ISettingsManager<T>
    {
        IObservable<T> LoadSettings();
        Task<bool> SaveSettings(T settings);
    }
}