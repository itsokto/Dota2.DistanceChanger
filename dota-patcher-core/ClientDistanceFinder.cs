using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dota.Patcher.Core.Abstractions;
using Dota.Patcher.Core.Models;

namespace Dota.Patcher.Core
{
    public class ClientDistanceFinder : IClientDistanceFinder
    {
        private readonly Regex _regex =
            new Regex(@"(?<=\0)([\d]{4,})(?=\0)", RegexOptions.Compiled | RegexOptions.RightToLeft);

        public IEnumerable<SearchResult<string>> Find(byte[] array, IEnumerable<byte[]> patterns)
        {
            foreach (var pattern in patterns)
            {
                var index = IndexOf(array, pattern);
                if (index >= 0)
                {
                    var (result, offset, distance) =
                        GetDistanceFromBytesInRange(array, index - 12, pattern.Length + 24);

                    if (!result) continue;

                    var realOffset = index + offset - 12;

                    yield return new SearchResult<string>
                    {
                        Offset = realOffset,
                        Value = distance
                    };
                }
            }
        }

        public IEnumerable<SearchResult<string>> Find(byte[] array, byte[] pattern)
        {
            return Find(array, new[] {pattern});
        }

        public IEnumerable<SearchResult<string>> Find(byte[] array, IEnumerable<int> offsets)
        {
            foreach (var offset in offsets)
            {
                var (result, _, distance) = GetDistanceFromBytesInRange(array, offset, 4);
                if (!result) continue;

                yield return new SearchResult<string>
                {
                    Offset = offset,
                    Value = distance
                };
            }
        }

        public string Find(byte[] array, int offset)
        {
            return Find(array, new[] {offset}).FirstOrDefault()?.Value;
        }

        private static int IndexOf(byte[] value, byte[] pattern)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            var valueLength = value.Length;
            var patternLength = pattern.Length;

            if (valueLength == 0 || patternLength == 0 || patternLength > valueLength)
                return -1;

            var badCharacters = new int[256];

            for (var i = 0; i < 256; ++i)
                badCharacters[i] = patternLength;

            var lastPatternByte = patternLength - 1;

            for (var i = 0; i < lastPatternByte; ++i)
                badCharacters[pattern[i]] = lastPatternByte - i;

            // Beginning

            var index = 0;

            while (index <= valueLength - patternLength)
            {
                for (var i = lastPatternByte; value[index + i] == pattern[i]; --i)
                    if (i == 0)
                        return index;

                index += badCharacters[value[index + lastPatternByte]];
            }

            return -1;
        }

        private static byte[] GetBytesFromArray(IEnumerable<byte> array, int offset, int count)
        {
            return array.Skip(offset).Take(count).ToArray();
        }

        private (bool result, int offsetInRange, string distance) GetDistanceFromBytesInRange(IEnumerable<byte> array,
            int offset, int count)
        {
            var originEncodedString = GetBytesFromArray(array, offset, count);
            var decodedString = Encoding.Default.GetString(originEncodedString);
            var match = _regex.Match(decodedString);

            return match.Success
                ? (true, match.Index, match.Value)
                : (false, -1, null);
        }
    }
}