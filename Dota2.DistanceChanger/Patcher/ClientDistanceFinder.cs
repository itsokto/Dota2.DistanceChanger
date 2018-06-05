using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dota2.DistanceChanger.Patcher.Abstractions;

namespace Dota2.DistanceChanger.Patcher
{
    public class ClientDistanceFinder : IClientDistanceFinder
    {
        private readonly Regex _regex = new Regex(@"(?<=\0)([\d]{4,})(?=\0)", RegexOptions.Compiled);

        public IDictionary<long, string> Get(byte[] array, IEnumerable<byte[]> patterns)
        {
            var dictionary = new Dictionary<long, string>();
            foreach (var pattern in patterns)
            {
                var index = IndexOf(array, pattern);
                if (index >= 0)
                {
                    var (result, offset, distance) =
                        GetDistanceFromBytesInRange(array, index - 12, pattern.Length + 24);

                    if (!result) continue;

                    var realOffset = index + offset - 12;
                    if (!dictionary.ContainsKey(realOffset))
                        dictionary.Add(realOffset, distance);
                }
            }

            return dictionary;
        }

        public IDictionary<long, string> Get(byte[] array, byte[] pattern)
        {
            return Get(array, new[] {pattern});
        }

        public IDictionary<long, string> Get(byte[] array, IEnumerable<long> offsets)
        {
            var dictionary = new Dictionary<long, string>();
            foreach (var offset in offsets)
            {
                var (result, _, distance) = GetDistanceFromBytesInRange(array, offset, 4);
                if (!result) continue;
                if (!dictionary.ContainsKey(offset))
                    dictionary.Add(offset, distance);
            }

            return dictionary;
        }

        public string Get(byte[] array, long offset)
        {
            return Get(array, new[] {offset}).Values.FirstOrDefault();
        }

        private static long IndexOf(byte[] value, byte[] pattern)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            var valueLength = value.LongLength;
            var patternLength = pattern.LongLength;

            if (valueLength == 0 || patternLength == 0 || patternLength > valueLength)
                return -1;

            var badCharacters = new long[256];

            for (long i = 0; i < 256; ++i)
                badCharacters[i] = patternLength;

            var lastPatternByte = patternLength - 1;

            for (long i = 0; i < lastPatternByte; ++i)
                badCharacters[pattern[i]] = lastPatternByte - i;

            // Beginning

            long index = 0;

            while (index <= valueLength - patternLength)
            {
                for (var i = lastPatternByte; value[index + i] == pattern[i]; --i)
                    if (i == 0)
                        return index;

                index += badCharacters[value[index + lastPatternByte]];
            }

            return -1;
        }

        private static byte[] GetBytesFromArray(byte[] array, long offset, long count)
        {
            var buffer = new byte[count];
            for (var i = 0; i < count; i++) buffer[i] = array[offset + i];

            return buffer;
        }

        private (bool result, long offsetInRange, string distance) GetDistanceFromBytesInRange(byte[] array,
            long offset, long count)
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