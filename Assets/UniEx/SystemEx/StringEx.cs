using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

/// <summary>
/// stringクラスの拡張メソッド群
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// null or Emptyをメンバ化. インスタンスがnullでも使える
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
	public static bool IsNullOrEmpty(this string source)
	{
		return string.IsNullOrEmpty(source);
	}

	/// <summary>
	/// string.Formatをメンバ化. "hoge{0}".Formats(1) の様に書ける
	/// </summary>
	/// <param name="format"></param>
	/// <param name="values"></param>
	/// <returns></returns>
	public static string Formats(this string format, params object[] values)
	{
		return string.Format(format, values);
	}
	/// <summary>
	/// valuesを表形式のstringにして返す.
	/// width: 列数. cellLength : 形式をきれいにするための1セルの横幅
	/// </summary>
	public static string TableFormat<T>(int width, int cellLength, params T[] values)
	{
		StringBuilder builder = new StringBuilder();

		foreach(var item in values.Select((v,i) => new {v,i}))
		{
			builder.Append("{{0,{0}}}".Formats(cellLength).Formats(item.v));

			if (item.i != values.Length - 1 && item.i % width == width - 1)
				builder.AppendLine("");
		}

		return builder.ToString();
	}
}
