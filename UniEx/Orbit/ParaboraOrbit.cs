using UnityEngine;
using System.Collections;
using System;

namespace UniEx
{
    [System.Serializable]
    public class ParaboraOrbit : Orbit2DNoOption
    {
        // カーブの度合い
        [SerializeField] float curveLength_;

        public float CurveLength
        {
            get { return curveLength_; }
            set { curveLength_ = value; }
        }

        public ParaboraOrbit(float curveLength)
        {
            CurveLength = curveLength;
        }

        protected override Vector2 NormalFunc(OrbitEdge edge, Vector2 currentPos, float normalizedTime)
        {
            // 長さ
            var length = edge.GetNormalizedNormal();
            var delta = length*CurveLength;
            delta *= AxisFunc.ParaboraUp(normalizedTime);
            return delta;
        }
    }
}