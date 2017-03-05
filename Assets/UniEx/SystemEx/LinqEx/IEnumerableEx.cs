using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace UniEx
{
    public static class IEnumerableEx
    {
        /// <summary>
        /// 最小値を持つ要素を返します
        /// </summary>
        public static TSource FindMin<TSource, TResult>(
            this IEnumerable<TSource> self,
            Func<TSource, TResult> selector)
        {
            var min = self.Min(selector);
            return self.First(c => selector(c).Equals(min));
        }

        /// <summary> 最小値を持つ要素を返す. 要素が空の時はdefaultを返す </summary>
        public static TSource FindMinOrDefault<TSource, TResult>(
            this IEnumerable<TSource> self,
            Func<TSource, TResult> selector)
        {
            return self.IsNullOrEmpty() ? default(TSource) : self.FindMin(selector);
        }

        /// <summary>
        /// 最大値を持つ要素を返します
        /// </summary>
        public static TSource FindMax<TSource, TResult>(
            this IEnumerable<TSource> self,
            Func<TSource, TResult> selector)
        {
            var max = self.Max(selector);
            return self.First(c => selector(c).Equals(max));
        }


        /// <summary> 最大値を持つ要素を返す. 要素が空の時はdefaultを返す </summary>
        public static TSource FindMaxOrDefault<TSource, TResult>(
            this IEnumerable<TSource> self,
            Func<TSource, TResult> selector)
        {
            return self.IsNullOrEmpty() ? default(TSource) : self.FindMax(selector);
        }

        /// <summary>
        /// 最大を持つ要素をすべて返す.　Where( x => x.Max()) と同義
        /// TResultがクラスの場合は注意
        /// </summary>
        public static IEnumerable<TSource> FindMaxAll<TSource, TResult>(this IEnumerable<TSource> self,
            Func<TSource, TResult> selector)
        {
            var max = self.Max(selector);
            return self.Where(c => selector(c).Equals(max));
        }

        /// <summary>
        /// 最小を持つ要素をすべて返す.　Where( x => x.Min()) と同義
        /// TResultがクラスの場合は注意
        /// </summary>
        public static IEnumerable<TSource> FindMinAll<TSource, TResult>(this IEnumerable<TSource> self,
            Func<TSource, TResult> selector)
        {
            var min = self.Min(selector);
            return self.Where(c => selector(c).Equals(min));
        }

        /// <summary>
        /// ランダムに一つを返す. 無い場合はデフォルト
        /// </summary>
        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> self)
        {
            return self.IsNullOrEmpty() ? default(TSource) : self.Random();
        }

        /// <summary>
        /// weightで指定した重みをもとに,ランダムに一つを返す. 無い場合はデフォルト
        /// </summary>
        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> self, Func<TSource, int> weight)
        {
            if (self.IsNullOrEmpty())
                return default(TSource);

            return self.Random();
        }

        /// <summary>
        /// ランダムに一つを返す. 無い場合は例外
        /// </summary>
        public static TSource Random<TSource>(this IEnumerable<TSource> self)
        {
            return self.ElementAt(RandomUtility.Range(0, self.Count()));
        }

        /// <summary>
        /// weightで指定した重みをもとに, ランダムに一つを返す. 無い場合は例外
        /// </summary>
        public static TSource Random<TSource>(this IEnumerable<TSource> self, Func<TSource, int> weight)
        {
            return self.ElementAt(RandomUtility.DiceToss(self.Select(weight)));
        }

        // 2条件あるソート
        public static void Sort2<TSource, TResult1, TResult2>(this List<TSource> self, Func<TSource, TResult1> selector1,
            Func<TSource, TResult2> selector2) where TResult1 : IComparable where TResult2 : IComparable
        {
            self.Sort((x, y) =>
            {
                var result = selector1(x).CompareTo(selector1(y));
                return result != 0 ? result : selector2(x).CompareTo(selector2(y));
            });
        }

        /// <summary>
        /// リスト内のオブジェクトを, keySelectorにより, クラスタリングする
        /// keySelectorで指定した値をKey, valueSelectorで指定した値をValueとした Dictionary<Key, List<Value>>型として返す
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static Dictionary<TKey, List<TValue>> Merge2Dictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> self, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            var ret = new Dictionary<TKey, List<TValue>>();

            foreach (var elem in self)
            {
                var key = keySelector(elem);

                if (ret.ContainsKey(key) == false)
                    ret.Add(key, new List<TValue>());

                ret[key].Add(valueSelector(elem));
            }

            return ret;
        }

        // HashSet用のAddRange
        // 存在しているものがあればfalseを返す
        public static bool AddRange<T>(this HashSet<T> self, IEnumerable<T> range)
        {
            return range.Aggregate(true, (current, e) => current & self.Add(e));
        }

        /// <summary>
        /// 要素が空 or nullかを返す拡張メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        {
            return self == null || !self.Any();
        }

        /// <summary>
        /// 要素が空でないか. IsNullOrEmpty() == falseと同値
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool NotEmpty<T>(this IEnumerable<T> self)
        {
            return self.IsNullOrEmpty() == false;
        }

        /// <summary>
        /// IEnumerable用のFindIndex
        /// </summary>
        public static int FindIndex<T>(this IEnumerable<T> self, Func<T, bool> predict)
        {
            var ret = self.Select((v, i) => new {v, i}).FirstOrDefault(x => predict(x.v));

            return ret == null ? -1 : ret.i;
        }

        /// <summary> predictを満たすインデックスをすべて取得する </summary>
        public static IEnumerable<int> FindAllIndex<T>(this IEnumerable<T> self, Func<T, bool> predict)
        {
            return self.Select((v, i) => new {v, i}).Where(x => predict(x.v)).Select(item => item.i);
        }

        public static int FindIndex<T>(this IEnumerable<T> self, T element)
        {
            return self.FindIndex(x => EqualityComparer<T>.Default.Equals(x, element));
        }
    }
}