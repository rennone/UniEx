using UnityEngine;
using System.Collections;

// 時間を加工する関数
[System.Serializable]
public class OrbitTimeFunc
{
	public static readonly OrbitTimeFunc Linear = new OrbitTimeFunc();

	// デフォルトは線形
	public virtual float TimeFunc(float normalizedTime)
	{
		return normalizedTime;
	}
}

[System.Serializable]
public class OrbitEasyTimeFunc : OrbitTimeFunc
{
	[SerializeField]
	public float coef_;

	public float Coef { get { return coef_; } set { coef_ = value; } }


	public float A { get { return 2 * C - 2; } }

	public float B { get { return -1.5f * A; } }

	public float C { get { return coef_; } }

	public override float TimeFunc(float normalizedTime)
	{
		var p = normalizedTime;
		var pp = p * p;
		var ppp = pp * p;
		return A * ppp + B * pp + C * p;
	}

	public OrbitEasyTimeFunc(float coef = 1.0f)
	{
		Coef = coef;
	}
}

[System.Serializable]
public class OrbitEasyTimeFunc2 : OrbitTimeFunc
{
	[SerializeField]
	public float coef_;

	[SerializeField]
	public float t_;


	public float A { get { return (1 - C) / ( 1 - 3 * t_); } }

	public float B { get { return -3 * A * t_; } }

	public float C { get { return coef_; } }

	public override float TimeFunc(float normalizedTime)
	{
		var p = normalizedTime;
		var pp = p * p;
		var ppp = pp * p;
		return A * ppp + B * pp + C * p;
	}
}

