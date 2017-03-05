using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 初期位置がターゲットからの移動差分で求められるEdge
/// </summary>
public class OrbitEdgeTransformTargetBase : OrbitEdge
{
	[SerializeField]
	Transform target_;
	public Transform Target { get { return target_; } set { target_ = value; } }

	// 目的地との差分を取得
	Vector2 startDelta_;

	// 目的地の差分
	[SerializeField]
	Vector2 targetDelta_;
	public Vector2 TargetDelta { get { return targetDelta_; } set { targetDelta_ = value; } }

	public override Vector2 Dest {	get	{ return Target.GetPos2D() + TargetDelta;}}

	// 目的地からの差分位置を返す
	public override Vector2 Start { get	{ return Dest + startDelta_; } }

	public OrbitEdgeTransformTargetBase(Vector2 start, Transform target, Vector2 targetDelta)
	{
		// ターゲットからの距離ベクトルを保存する
		startDelta_ = start - target.GetPos2D();

		// ターゲットの設定
		Target = target;

		TargetDelta = targetDelta;
	}

	public OrbitEdgeTransformTargetBase(Vector2 start, Transform target)
		:this(start, target, Vector2.zero)
	{
	}
}
