using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Extensions;
using Dota2.DistanceChanger.Core.Models;
using Dota2.Patcher.Core.Abstractions;

namespace Dota2.DistanceChanger.Core.Infrastructure
{
	public class DotaClientDistancePatcher : IDotaClientDistancePatcher
	{
		private readonly IBackupManager _backupManager;

		private readonly IDotaClientDistance _dotaClientDistance;

		public DotaClientDistancePatcher(IBackupManager backupManager, IDotaClientDistance dotaClientDistance)
		{
			_backupManager = backupManager;
			_dotaClientDistance = dotaClientDistance;
		}

		public Task PatchAsync(Settings settings)
		{
			return new[] { settings.X32Client, settings.X64Client }.ForEachAsync(async client =>
			{
				var fullPath = settings.Dota2FolderPath + client.LocalPath;

				if (settings.Backup)
				{
					await _backupManager.CreateBackupAsync(fullPath, $"{fullPath}.back{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
				}

				await _dotaClientDistance.SetAsync(fullPath, client.Distance.Value, client.Distance.Offset);
			});
		}
	}
}