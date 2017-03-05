using UnityEngine;
using System.Collections.Generic;

namespace UniEx
{
    public static class DictionaryEx
    {
        /// <summary>
        /// TryGetValueのラッパー. 無い場合はdefault(TValue)を返す
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source.IsNullOrEmpty())
                return default(TValue);

            TValue result;
            return source.TryGetValue(key, out result) ? result : default(TValue);
        }
    }
}