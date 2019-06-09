using MvvmCross.Core;

namespace Dota2.DistanceChanger
{
    public partial class App
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<AppSetup>();
        }
    }
}