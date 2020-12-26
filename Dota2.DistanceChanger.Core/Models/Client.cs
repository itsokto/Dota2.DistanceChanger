using System.Diagnostics;
using Dota2.Patcher.Core.Models;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Dota2.DistanceChanger.Core.Models
{
	[DebuggerDisplay("[DisplayName = {DisplayName}, Distance = {Distance}]")]
	[JsonObject(MemberSerialization.OptOut)]
	public class Client : ReactiveObject
	{
		[Reactive]
		public SearchResult<int> Distance { get; set; }

		[Reactive]
		public string DisplayName { get; set; }

		[Reactive]
		public string LocalPath { get; set; }
	}
}