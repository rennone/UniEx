using UnityEngine;


namespace UniEx
{
    [System.Serializable]
    public class RotOrbit : Orbit2DNoOption
    {
        [SerializeField] float rotNum_;

        public float RotateNum
        {
            get { return rotNum_; }
            private set { rotNum_ = value; }
        }

        // TODO :
        // Inspectorでリアルタイムで書き換えられるように毎回計算しているが
        // キャッシュした方が効率良い
        public float Theta
        {
            get { return Mathf.PI*2*RotateNum; }
        }

        // 半径の取得
        protected virtual float GetRadius(float normalizedTime)
        {
            return 1 - normalizedTime;
        }

        protected override Vector2 TangentFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            var x = -edge.GetDirection();
            return GetRadius(normalizedTime)*Mathf.Cos(normalizedTime*Theta)*x - x;
        }

        protected override Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            // directionの符号が反転しているので, 時計回りにするために反時計回りの法線を取得
            var y = edge.GetCounterClockWiseNormal();
            return GetRadius(normalizedTime)*Mathf.Sin(normalizedTime*Theta)*y;
        }

        public RotOrbit()
        {
        }

        public RotOrbit(float rotNum, float maxRadius)
        {
            RotateNum = rotNum;
        }
    }
}
