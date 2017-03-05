﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class OrbitAnimationCurveTimeFunc : OrbitTimeFunc
{
	[SerializeField]
	AnimationCurve animationCurve_ = new AnimationCurve();

	public override float TimeFunc(float normalizedTime)
	{
		return animationCurve_.Evaluate(normalizedTime);
	}
}
