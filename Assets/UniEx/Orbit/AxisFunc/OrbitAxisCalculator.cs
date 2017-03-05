using UnityEngine;
using System.Collections;
using System;

// 現在地→目的地のベクトルとx軸とした場合の
// x成分を計算する関数( 0 <= p <= 1 )
// y成分を計算する関数( 0 <= p <= 1 )
// f(0) != 0 だと, スタート位置がずれる
// f(1) != 1 だと, ずれた位置に到達する

// v = [ x(p), y(p) ] で現在位置は計算される
// (0,0) : 出発地, (1, 1) 目的地

// x(p) = p, y(p) = 0 だと直線
// x(p) = 9, y(p) = c(p - 0.5)^2 - 0.25c だと放物線となる

public abstract class OrbitAxisCalculator
{
	// 接線方向の
	public abstract Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime);
	public abstract Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime);
}

// 任意の関数を入れる
public abstract class AnyOrbitAxisCalculator : OrbitAxisCalculator
{
	protected Func<OrbitEdge, Vector2, float, Vector2> tangentFunc_;
	protected Func<OrbitEdge, Vector2, float, Vector2> normalFunc_;

	public override Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return normalFunc_ != null ? normalFunc_(edge, currentPos, normalizedTime) : Vector2.zero;
	}

	public override Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return tangentFunc_ != null ? tangentFunc_(edge, currentPos, normalizedTime) : Vector2.zero;
	}
}
