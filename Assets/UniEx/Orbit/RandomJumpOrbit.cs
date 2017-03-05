using UnityEngine;
using System.Collections;
using System;

namespace UniEx
{
    [Serializable]
    public class RandomJumpOrbit : Orbit2D<float>
    {
        // 最小のジャンプ量
        [SerializeField] float minJumpLength_;

        public float MinJumpLength
        {
            get { return minJumpLength_; }
            set { minJumpLength_ = value; }
        }

        // 最大のジャンプ量
        [SerializeField] float maxJumpLength_;

        public float MaxJumpLength
        {
            get { return maxJumpLength_; }
            set { maxJumpLength_ = value; }
        }

        public RandomJumpOrbit(float min = 0, float max = 1)
        {
            MinJumpLength = min;
            MaxJumpLength = max;
        }


        // オプション値は, 今回の軌道で使うジャンプ量. min ~ maxの間でランダムに生成する
        protected override float CreateOption(OrbitEdge edge, OrbitTimeFunc timeFunc, float time)
        {
            return RandomUtility.Range(MinJumpLength, MaxJumpLength);
        }

        /// <summary>
        ///  放物線を描く
        /// </summary>
        protected override Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime, float option)
        {
            return Vector2.up*AxisFunc.ParaboraUp(normalizedTime)*option;
        }
    }
}