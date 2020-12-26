using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Abstractions
{
	public interface IUserDialogs
	{
		void Alert(object content);

		Task<string> OpenFileDialog();

		Task<string> OpenFolderDialog();
	}
}