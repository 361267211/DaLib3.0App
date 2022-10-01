using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace TaskManager.Tasks.Extension
{
    public static class ListExtensions
    {
        public static ConcurrentBag<T> ToConcurrentBag<T>(this List<T> source)
        {
            var result = new ConcurrentBag<T>();
            foreach (var item in source)
            {
                result.Add(item);
            }
            return result;
        }

        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this List<TSource> source,
                      Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var result = new ConcurrentDictionary<TKey, TElement>();
            foreach (var item in source)
            {
                result.TryAdd(keySelector(item), elementSelector(item));
            }
            return result;
        }
    }
}
