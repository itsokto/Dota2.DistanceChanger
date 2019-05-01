using System;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Models;

namespace Dota2.DistanceChanger.Abstractions
{
    public interface ISettingsManager
    {
        IObservable<Settings> LoadSettings();
        Task<bool> SaveSettings(Settings settings);
    }
}