using DynamicData.Binding;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Dota2.DistanceChanger.Core.Models
{
	[JsonObject(MemberSerialization.OptOut)]
	public class Settings : ReactiveObject
	{
		[Reactive]
		public bool Backup { get; set; }

		[Reactive]
		public Client X32Client { get; set; }

		[Reactive]
		public Client X64Client { get; set; }

		[Reactive]
		public bool DarkMode { get; set; }

		[Reactive]
		public string Dota2FolderPath { get; set; }

		[Reactive]
		public ObservableCollectionExtended<byte[]> Patterns { get; set; }
	}
}