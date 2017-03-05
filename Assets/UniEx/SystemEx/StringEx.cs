using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

namespace UniEx
{
    /// <summary>
    /// stringクラスの拡張メソッド群
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// null or Emptyをメンバ化. 引数がnullでも使える
        /// </summary>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// string.Formatをメンバ化. "hoge{0}".Formats(1) の様に書ける
        /// </summary>
        public static string Formats(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        /// <summary>
        /// valuesをvalues.Length/width行, width列の表形式のstringにして返す.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">列数</param>
        /// <param name="cellLength">1セルの横幅</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string TableFormat<T>(int width, int cellLength, params T[] values)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in values.Select((v, i) => new {v, i}))
            {
                builder.Append("{{0,{0}}}".Formats(cellLength).Formats(item.v));

                if (item.i != values.Length - 1 && item.i%width == width - 1)
                    builder.AppendLine("");
            }

            return builder.ToString();
        }
    }
}