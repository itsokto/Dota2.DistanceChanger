using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dota2.Patcher.Core.Abstractions;
using Dota2.Patcher.Core.Models;

namespace Dota2.Patcher.Core
{
	public class DotaClientDistanceParser : IDotaClientDistanceParser
	{
		private readonly Regex _regex;

		public DotaClientDistanceParser()
		{
			_regex = new Regex(@"([\d]{4,})", RegexOptions.Compiled | RegexOptions.RightToLeft);
		}

		public IEnumerable<SearchResult<int>> Get(byte[] array, IEnumerable<byte[]> patterns)
		{
			foreach (var pattern in patterns)
			{
				var index = IndexOf(array, pattern);
				if (index >= 0)
				{
					var (result, matchIndex, distance) = ParseDistance(array, index - 12, pattern.Length + 24);

					if (!result) continue;

					var offset = index + matchIndex - 12;

					yield return new SearchResult<int>
					{
						Offset = offset,
						Value = distance
					};
				}
			}
		}

		public IEnumerable<SearchResult<int>> Get(byte[] array, byte[] pattern)
		{
			return Get(array, new[] { pattern });
		}

		public SearchResult<int> Get(byte[] array, int offset)
		{
			var (_, index, distance) = ParseDistance(array, offset, 4);

			return new SearchResult<int>
			{
				Offset = index,
				Value = distance
			};
		}

		private (bool result, int index, int distance) ParseDistance(byte[] array, int offset, int count)
		{
			var encodedString = array.Skip(offset).Take(count).ToArray();
			var decodedString = Encoding.UTF8.GetString(encodedString);
			var match = _regex.Match(decodedString);

			return match.Success && int.TryParse(match.Value, out var value)
				? (true, match.Index, value)
				: (false, -1, -1);
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
	}
}