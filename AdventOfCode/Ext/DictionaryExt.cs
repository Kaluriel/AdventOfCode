using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Ext
{
    public static class DictionaryExt
    {
        public static void AddRange<T, U>(this Dictionary<T, U> source, IEnumerable<KeyValuePair<T, U>> items)
        {
            foreach (var item in items)
            {
                source.Add(item.Key, item.Value);
            }
        }
    }
}
