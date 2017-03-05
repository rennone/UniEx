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

public abstract class AxisFunc
{
	public abstract float XAxisFunc(float p);
	public abstract float YAxisFunc(float p);
	
	// 接線方向の
	public abstract Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime);
	public abstract Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime);


	public static float Linear(float p) { return p; }
	public static float Zero(float p) { return 0; }
	public static float One(float p) { return 1; }

	// 上に凸の放物線
	public static float ParaboraUp(float p)
	{
		return p * (1 - p);
	}

	// 下に凸の放物線
	public static float ParaboraDown(float p)
	{
		return 1 - ParaboraUp(p);
	}
}

// 任意の関数を入れる
public class AnyAxisFunc : AxisFunc
{
	public Func<float, float> XAxis { get; private set; }
	public Func<float, float> YAxis { get; private set; }


	public Func<OrbitEdge, Vector2, float, Vector2> Tangent { get; set; }
	public Func<OrbitEdge, Vector2, float, Vector2> Normal { get; set; }

	public override float XAxisFunc(float p)
	{
		return XAxis(p);
	}

	public override float YAxisFunc(float p)
	{
		return YAxis(p);
	}

	public override Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return Tangent(edge, currentPos, normalizedTime);
	}

	public override Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return Normal(edge, currentPos, normalizedTime);
	}

	public AnyAxisFunc(Func<float, float> xAxisFunc, Func<float, float> yAxisFunc)
	{
		XAxis = xAxisFunc;
		YAxis = yAxisFunc;
	}

}

// 直線軌道
public class DirectAxisFunc : AxisFunc
{
	public override float XAxisFunc(float p)
	{
		return Linear(p);
	}

	public override float YAxisFunc(float p)
	{
		return Zero(p);
	}

	public override Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return edge.GetDirection() * normalizedTime;
	}

	public override Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return Vector2.zero;
	}
}

// 放物線軌道
public class ParabolaAxisFunc : AxisFunc
{
	float coef_ = 1.0f;

	public ParabolaAxisFunc(float coef = 1.0f)
	{
		coef_ = coef;
	}

	public override float XAxisFunc(float p)
	{
		return Linear(p);
	}

	public override float YAxisFunc(float p)
	{
		return ParaboraUp(p) * coef_;
	}

	public override Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return edge.GetDirection() * normalizedTime;
	}

	public override Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		var n = edge.GetNormal().normalized;

		return  n *ParaboraUp(normalizedTime) * coef_;
	}
}

// ワープ移動
public class WarpAxisFunc : AxisFunc
{
	readonly float threashold_;

	public WarpAxisFunc(float threashold = 0)
	{
		threashold_ = threashold;
	}

	public override float XAxisFunc(float p)
	{
		return  p < threashold_ ? 0 : 1;
	}

	public override float YAxisFunc(float p)
	{
		return Zero(p);
	}

	public override Vector2 TangentFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return edge.Dest;
	}

	public override Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
	{
		return Vector2.zero;
	}
}

