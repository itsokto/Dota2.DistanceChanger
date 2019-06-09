using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dota2.DistanceChanger.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> selector)
        {
            return Task.WhenAll(source.Select(selector));
        }

        public static async Task<IEnumerable<TOut>> ForEachAsync<TIn, TOut>(this IEnumerable<TIn> source,
            Func<TIn, Task<TOut>> selector)
        {
            return await Task.WhenAll(source.Select(selector));
        }
    }
}