using System;
using Dota2.DistanceChanger.Core.Abstractions;
using MaterialDesignThemes.Wpf;

namespace Dota2.DistanceChanger.Infrastructure
{
    public class UserInterface : IUserInterface
    {
        private static readonly PaletteHelper PaletteHelper = new PaletteHelper();

        public void DarkMode(bool enable)
        {
            PaletteHelper.SetLightDark(enable);
        }

        public void SetPrimaryColor(string color)
        {
            throw new NotImplementedException();
        }
    }
}