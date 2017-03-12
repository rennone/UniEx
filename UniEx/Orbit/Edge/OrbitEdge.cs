using UnityEngine;
using System.Collections;

namespace UniEx
{
    public abstract class OrbitEdge
    {
        public abstract Vector2 Start { get; }
        public abstract Vector2 Dest { get; }

        // スタートからゴールへのベクトル
        public Vector2 GetDirection()
        {
            return Dest - Start;
        }

        // 正規化された接線
        public Vector2 GetNormalizedDirection()
        {
            var dir = GetDirection();
            return dir.normalized;
        }

        // 法線
        public Vector2 GetNormal()
        {
            var dir = GetDirection();

            // 時計回り
            return new Vector2(dir.y, -dir.x);
        }

        // 正規化された法線
        public Vector2 GetNormalizedNormal()
        {
            var nor = GetNormal();

            return nor.normalized;
        }

        // Directionに対して時計回りの法線
        public Vector2 GetClockWiseNormal()
        {
            var dir = GetDirection();

            // 時計回り
            return new Vector2(dir.y, -dir.x);
        }

        // 反時計回りの法線
        public Vector2 GetCounterClockWiseNormal()
        {
            return -GetClockWiseNormal();
        }
    }

    public class OrbitEdgeConstant : OrbitEdge
    {
        Vector2 start_;

        public override Vector2 Start
        {
            get { return start_; }
        }

        Vector2 dest_;

        public override Vector2 Dest
        {
            get { return dest_; }
        }

        public OrbitEdgeConstant(Vector2 start, Vector2 dest)
        {
            start_ = start;
            dest_ = dest;
        }
    }

}