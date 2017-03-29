using UnityEngine;

namespace UniEx
{
    [System.Serializable]
    public class WaveOrbit : Orbit2DNoOption
    {
        [SerializeField] float amptitude_;

        public float Amptitude
        {
            get { return amptitude_; }
            set { amptitude_ = value; }
        }

        [SerializeField] float hz_;

        public float Hz
        {
            get { return hz_; }
            set { hz_ = value; }
        }

        public WaveOrbit(float amptitude, float hz)
        {
            Amptitude = amptitude;
            Hz = hz;
        }

        protected override Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            var a = Amptitude*Mathf.Sin(Hz*normalizedTime*Mathf.PI*2);
            var n = edge.GetNormalizedNormal();
            return a*n;
        }
    }
}