using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;

namespace Dota2.DistanceChanger
{
    public partial class App
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}