using System.Reactive.Disposables;
using Dota2.DistanceChanger.Core.ViewModels;
using ReactiveUI;

namespace Dota2.DistanceChanger.Views
{
	public partial class RootWindow
	{
		public RootWindow()
		{
			InitializeComponent();

			ViewModel = new AppSetup();

			this.WhenActivated(disposable =>
			{
				this.Bind(ViewModel, model => model.Router, window => window.RoutedViewHost.Router).DisposeWith(disposable);
            });
		}
	}
}