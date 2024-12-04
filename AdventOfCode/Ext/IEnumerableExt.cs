using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Ext
{
	public static class IEnumerableExt
	{
        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// https://stackoverflow.com/questions/419019/split-list-into-sublists-with-linq
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            return Chunk(source, chunksize, chunksize);
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize, int stride)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(stride);
            }
        }

        public static IEnumerable<U> SelectWithNext<T, U>(this IEnumerable<T> source, Func<T, T, U> func, bool includeLast = false)
        {
            T prev = source.FirstOrDefault();
            source = source.Skip(1);

            foreach (var item in source)
            {
                yield return func(prev, item);
                prev = item;
            }

            if (includeLast)
            {
                yield return func(prev, default);
            }
        }

        public static IEnumerable<IEnumerable<T>> PermutationsOfOneRemoved<T>(this IEnumerable<T> source)
        {
            return Enumerable
                .Range(0, source.Count())
                .Select(i => source
                    .Take(i)
                    .Concat(source.Skip(i + 1))
                );
        }
    }
}
