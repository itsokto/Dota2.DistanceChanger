using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Async.IO.Abstractions;
using Dota2.DistanceChanger.Core.Abstractions;
using Dota2.DistanceChanger.Core.Extensions;
using Dota2.DistanceChanger.Core.Models;
using Dota2.Patcher.Core.Abstractions;
using Dota2.Patcher.Core.Models;

namespace Dota2.DistanceChanger.Core.Infrastructure
{
    public class DotaClientDistanceLoader : IDotaClientDistanceLoader
    {
        private readonly IDistancePatcher _distancePatcher;
        private readonly IAsyncFile _asyncFile;

        public DotaClientDistanceLoader(IDistancePatcher distancePatcher, IAsyncFile asyncFile)
        {
            _distancePatcher = distancePatcher;
            _asyncFile = asyncFile;
        }

        public async Task<Settings> LoadAsync(Settings settings)
        {
            await new[] { settings.X32Client, settings.X64Client }.ForEachAsync(async client =>
            {
                var fullPath = settings.Dota2FolderPath + client.LocalPath;

                if (!_asyncFile.Exists(fullPath))
                {
                    throw new FileNotFoundException(fullPath);
                }

                var searchResult = new SearchResult<int>();

                if (client.Distance?.Offset > 0)
                {
                    searchResult = await _distancePatcher.GetAsync(fullPath, client.Distance.Offset)
                        .ConfigureAwait(false);
                }

                if (searchResult.Offset <= 0)
                {
                    var searchResults = await _distancePatcher.GetAsync(fullPath, settings.Patterns)
                        .ConfigureAwait(false);

                    //TODO: handle case when nothing was found
                    searchResult = searchResults.FirstOrDefault(x => x.Offset > 0);
                }

                client.Distance = searchResult;
            }).ConfigureAwait(false);

            return settings;
        }
    }
}