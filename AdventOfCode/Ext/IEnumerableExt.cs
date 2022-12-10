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
    }
}
