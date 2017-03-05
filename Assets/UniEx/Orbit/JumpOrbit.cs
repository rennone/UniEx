using UnityEngine;
using System.Collections;

namespace UniEx
{
    /// <summary>
    /// ParaboraOrbitと違い, 真上に放物線を描く
    /// </summary>
    [System.Serializable]
    public class JumpOrbit : Orbit2DNoOption
    {
        // ジャンプ量
        [SerializeField] float jumpLength_;

        public float JumpLength
        {
            get { return jumpLength_; }
            set { jumpLength_ = value; }
        }

        public JumpOrbit(float jumpLength)
        {
            JumpLength = jumpLength;
        }

        protected override Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return Vector2.up*JumpLength*AxisFunc.ParaboraUp(normalizedTime);
        }
    }
}