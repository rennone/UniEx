using UnityEngine;

namespace UniEx
{
    // ワープ移動用の軌跡
    [System.Serializable]
    public class WarpOrbit : Orbit2DNoOption
    {
        // どのタイミングでワープするか 0 ~ 1
        [SerializeField] float threshold_ = 0f;

        public WarpOrbit(float normalizedThresholdTime = 0f)
        {
            threshold_ = normalizedThresholdTime;
        }

        // threshold以下なら移動しない, 以上なら一気に移動する
        protected override Vector2 TangentFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return normalizedTime < threshold_ ? Vector2.zero : edge.GetDirection();
        }
    }
}