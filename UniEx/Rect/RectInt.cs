using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;

// UnityのRectの整数版
// 回転には対応しない
//public class RectInt
//{
//	public static readonly RectInt Zero = new RectInt(new Point<int>(0, 0), new Point<int>(0, 0));
//	/*
//		   ----- ----- ----- ---- (max)
//		  |     |     |     |    |
//		   ----- ----- ----- ----
//  	      |     |     |     |    |
//		(min)--- ----- ----- ----

//		min = (0,0). size = (4, 2)
//	*/
//	/// <summary>
//	/// 矩形の右下のセルの右上の位置
//	/// (0,0)を起点に, 3×3 の矩形では, Min = (0,0)となる((2,2)ではない )
//	/// </summary>
//	public Point<int> Min { get; set; }

//	/// <summary>
//	/// 矩形の右下のセルの右上の位置
//	/// (0,0)を起点に, 3×3 の矩形では, Max = (3,3)となる((2,2)ではない )
//	/// </summary>
//	public Point<int> Max { get; set; }

//	// サイズ
//	public int Width { get { return Max.x_ - Min.x_; } }
//	public int Height { get { return Max.y_ - Min.y_; } }
//	public Point<int> Size { get { return new Point<int>(Width, Height); } }

//	public RectInt(Point<int> min, Point<int> size)
//	{
//		Assert.IsTrue(size.x_ >= 0 && size.y_ >= 0);
//		Min = min;
//		Max = new Point<int>(min.x_ + size.x_, min.y_ + size.y_);
//	}

//	// pが矩形の範囲内にあるかどうか
//	public bool Contains(Point<int> p)
//	{
//		return Contains(p.x_, p.y_);
//	}

//	public bool Contains(int x, int y)
//	{
//		return Min.x_ <= x && Min.y_ <= y && x < Max.x_ && y < Max.y_;
//	}

//	// 重なり部分のRectを求める
//	public static RectInt Overlapped(RectInt a, RectInt b)
//	{
//		RectInt ret;
//		TryOverlapped(a, b, out ret);
//		return ret;
//	}

//	/// <summary>
//	/// Clamp p in RectInt
//	///  Min <= p < Max
//	/// </summary>
//	/// <param name="p"></param>
//	/// <returns></returns>
//	public Point<int> Clamp(Point<int> p)
//	{
//		return Clamp(p.x_, p.y_);
//	}

//	public Point<int> Clamp(int x, int y)
//	{
//		return new Point<int>(Mathf.Clamp(x, Min.x_, Max.x_ - 1), Mathf.Clamp(y, Min.y_, Max.y_ - 1));
//	}
	

//	// 重なり部分のRectを求める
//	public static bool TryOverlapped(RectInt a, RectInt b, out RectInt ret)
//	{
//		// どっちかnullなら重なり無し
//		if (a == null || b == null)
//		{
//			ret = Zero;
//			return false;
//		}

//		var minX = Mathf.Max(a.Min.x_, b.Min.x_);
//		var maxX = Mathf.Min(a.Max.x_, b.Max.x_);

//		var minY = Mathf.Max(a.Min.y_, b.Min.y_);
//		var maxY = Mathf.Min(a.Max.y_, b.Max.y_);

//		if (minX >= maxX || minY >= maxY)
//		{
//			ret = Zero;
//			return false;
//		}

//		ret = new RectInt(new Point<int>(minX, minY), new Point<int>(maxX - minX, maxY - minY));
//		return true;
//	}

//	// a と bのブーリアン減算を行い.
//	// aの内部かつ, bの外部となる領域を
//	// RectIの配列で返す
//	// 順番は 左, 右, 上(左右カット), 下(左右カット) の順番
//	// 重なってない場合はaが, bがaを包括している場合は空配列が返る
//	public static RectInt[] Difference(RectInt a, RectInt b)
//	{
//		RectInt lap;

//		// 重ならない場合はaを返す
//		if(TryOverlapped(a, b, out lap) == false)
//		{
//			// aがnullの場合は空
//			return a == null ? new RectInt[] { } :  new RectInt[] { a };
//		}

//		List<RectInt> ret = new List<RectInt>();

//		// 左側
//		if (a.Min.x_ < b.Min.x_)
//		{
//			ret.Add(new RectInt(a.Min, new Point<int>(b.Min.x_ - a.Min.x_, a.Height)));
//		}

//		// 右側
//		if (a.Max.x_ > b.Max.x_)
//		{
//			ret.Add(new RectInt(new Point<int>(b.Max.x_, a.Min.y_), new Point<int>(a.Max.x_ - b.Max.x_, a.Height)));
//		}

//		// 上側(左右カット)
//		if( a.Max.y_ > lap.Max.y_ )
//		{
//			ret.Add(new RectInt(new Point<int>(lap.Min.x_, lap.Max.y_), new Point<int>(lap.Width, a.Max.y_ - lap.Max.y_)));
//		}

//		if( a.Min.y_ < lap.Min.y_)
//		{
//			ret.Add(new RectInt(new Point<int>(lap.Min.x_, a.Min.y_), new Point<int>(lap.Width, lap.Min.y_ - a.Min.y_)));
//		}

//		return ret.ToArray();
//	}

//	// 範囲内のインデックスをEnumerableで返す
//	public IEnumerable<Point<int>> Range()
//	{
//		for(int x = Min.x_; x < Max.x_; ++x)
//		{
//			for(int y = Min.y_; y < Max.y_; ++y)
//			{
//				yield return new Point<int>(x, y);
//			}
//		}
//	}
	
//	public static bool operator ==(RectInt lhs, RectInt rhs)
//	{
//		// 両方null もしくは同じインスタンスチェック
//		if (System.Object.ReferenceEquals(lhs, rhs))
//			return true;
		
//		// どっちか一方がnullの場合はfalse
//		if ( (object)lhs == null || (object)rhs == null)
//			return false;

//		return lhs.Min == rhs.Min && lhs.Max == rhs.Max;
//	}

//	public static bool operator !=(RectInt lhs, RectInt rhs)
//	{
//		return !(lhs == rhs);
//	}

//	public override bool Equals(object obj)
//	{
//		if (obj == null)
//			return false;

//		var p = obj as RectInt;
//		if (p == null)
//			return false;

//		return this == p;
//	}

//	public override int GetHashCode()
//	{
//		// TODO : 適当
//		return Min.GetHashCode() ^ Max.GetHashCode();
//	}

//	public override string ToString()
//	{
//		return string.Format("min = {0}, max = {1}, size = {2}", Min, Max, Size);
//	}
//}