using UnityEngine;

namespace UniEx
{
    public static class Vector2Ex
    {

        public static Vector2 RotateTo(Vector2 from, Vector2 to, float maxRad)
        {
            var angle = Vector2.Angle(from, to);
            var maxDeg = Mathf.Rad2Deg*maxRad;
            if (angle < maxDeg)
                return to;

            var cross = from.x*to.y - from.y*to.x;

            var rad = cross > 0 ? maxRad : -maxRad;
            var s = Mathf.Sin(rad);
            var c = Mathf.Cos(rad);

            return new Vector2(from.x*c - from.y*s, from.x*s + from.y*c);
        }

        public static float ModDiff(float a, float b, float mod)
        {
            a = a%mod;
            b = b%mod;

            if (a < 0)
                a += mod;

            if (b < 0)
                b += mod;

            var ret = a - b;

            if (Mathf.Abs(ret) > mod/2)
            {
                var c = mod - Mathf.Abs(ret);

                if (ret < 0)
                    return c;
                return ret < 0 ? c : -c;
            }

            return ret;
        }

        public static Vector2 RotateTo2(Vector2 from, Vector2 to, float maxRad)
        {
            var fRad = Mathf.Atan2(from.y, from.x);

            var tRad = Mathf.Atan2(to.y, to.x);

            var deltaRad = ModDiff(tRad, fRad, Mathf.PI*2);

            var rad = tRad;

            if (Mathf.Abs(deltaRad) > maxRad)
                rad = fRad + (deltaRad < 0 ? -maxRad : maxRad);

            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        /// <summary> 時計回りに回転した結果を返す </summary>
        public static Vector2 Rotate(this Vector2 self, float degree)
        {
            var rad = Mathf.Deg2Rad*degree;
            var si = Mathf.Sin(rad);
            var co = Mathf.Cos(rad);

            var x = self.x*co - self.y*si;
            var y = self.y*co + self.x*si;

            return new Vector2(x, y);
        }

        // 2Dの外積
        public static float Cross2D(Vector2 a, Vector2 b)
        {
            return a.x*b.y - a.y*b.x;
        }

        /// <summary>
        /// 緯度経度に変換(x => 経度, y => 緯度)
        /// </summary>
        //public static Location ToLocation(this Vector2 self)
        //{
        //	return new Location(self.y, self.x);
        //}

        //public static Point<int> ToPoint(this Vector2 self)
        //{
        //	return new Point<int>((int)self.x, (int)self.y);
        //}

        /// <summary> xを入れ替えたものを返す。非破壊 </summary>
        public static Vector2 PutX(this Vector2 self, float x)
        {
            self.x = x;
            return self;
        }

        /// <summary> yを入れ替えたものを返す。非破壊 </summary>
        public static Vector2 PutY(this Vector2 self, float y)
        {
            self.y = y;
            return self;
        }

        /// <summary> xを入れ替えたものを返す。非破壊 </summary>
        public static Vector3 PutX(this Vector3 self, float x)
        {
            self.x = x;
            return self;
        }

        /// <summary> yを入れ替えたものを返す。非破壊 </summary>
        public static Vector3 PutY(this Vector3 self, float y)
        {
            self.y = y;
            return self;
        }

        /// <summary> zを入れ替えたものを返す。非破壊 </summary>
        public static Vector3 PutZ(this Vector3 self, float z)
        {
            self.z = z;
            return self;
        }
    }
}