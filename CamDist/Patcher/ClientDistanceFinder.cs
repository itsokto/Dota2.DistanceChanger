using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CamDist.Patcher.Abstractions;

namespace CamDist.Patcher
{
    public class ClientDistanceFinder : IClientDistanceFinder
    {
        public IDictionary<long, string> Find(byte[] array, IEnumerable<byte[]> patterns)
        {
            var dictionary = new Dictionary<long, string>();
            foreach (var pattern in patterns)
            {
                var index = IndexOf(array, pattern);
                if (index >= 0)
                {
                    var originEncodedString = GetBytesFromArray(array, index - 12, pattern.Length + 24);
                    var decodedString = Encoding.Default.GetString(originEncodedString);
                    var regex = new Regex(@"(?<=\0)([\d]{4,})(?=\0)");
                    var match = regex.Match(decodedString);
                    if (match.Success)
                    {
                        dictionary.Add(index + match.Index - 12, match.Value);
                    }
                }
            }
            return dictionary;
        }
        public static long IndexOf(byte[] value, byte[] pattern)
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
        public static byte[] GetBytesFromArray(byte[] array, long index, long count)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = array[index + i];
            }

            return buffer;
        }
    }
}
