using UnityEngine;
using System.Collections;
using System;

namespace UniEx
{
    [System.Serializable]
    public class OrbitEdgeTransform : OrbitEdge
    {
        [SerializeField] Transform target_;

        public Transform Target
        {
            get { return target_; }
            set { target_ = value; }
        }

        [SerializeField] Vector2 start_;

        // 目的地誤差分
        [SerializeField] Vector2 targetDelta_;

        public Vector2 TargetDelta
        {
            get { return targetDelta_; }
            set { targetDelta_ = value; }
        }

        // 目的地
        public override Vector2 Dest
        {
            get { return (Vector2) Target.position + TargetDelta; }
        }

        // 出発地
        public override Vector2 Start
        {
            get { return start_; }
        }

        // 再設定
        public void Set(Vector2 start, Transform target)
        {
            start_ = start;
            Target = target;
        }

        public void Set(Vector2 start, Transform target, Vector2 targetDelta)
        {
            start_ = start;
            Target = target;
            TargetDelta = targetDelta;
        }

        public OrbitEdgeTransform(Vector2 start, Transform target)
        {
            Set(start, target, Vector2.zero);
        }

        public OrbitEdgeTransform(Vector2 start, Transform target, Vector2 targetDelta)
        {
            Set(start, target, targetDelta);
        }

        public OrbitEdgeTransform()
        {
        }
    }
}