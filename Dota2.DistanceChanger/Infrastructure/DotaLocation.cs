using System.Threading.Tasks;
using Dota2.DistanceChanger.Core.Abstractions;
using Microsoft.Win32;

namespace Dota2.DistanceChanger.Infrastructure
{
    public class DotaLocation : IDotaLocation
    {
        public Task<string> GetAsync()
        {
            var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 570");

            var value = key?.GetValue("InstallLocation");

            return Task.FromResult(value?.ToString());
        }
    }
}