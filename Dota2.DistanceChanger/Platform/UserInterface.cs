using System;
using Dota2.DistanceChanger.Core.Platforms.Shared;
using MaterialDesignThemes.Wpf;

namespace Dota2.DistanceChanger.Platform
{
	public class UserInterface : IUserInterface
	{
		private static readonly PaletteHelper PaletteHelper = new();

		public void DarkMode(bool enable)
		{
			var theme = PaletteHelper.GetTheme();

			theme.SetBaseTheme(enable
				? Theme.Dark
				: Theme.Light);

			PaletteHelper.SetTheme(theme);
		}

		public void SetPrimaryColor(string color)
		{
			throw new NotImplementedException();
		}
	}
}