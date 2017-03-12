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
        
        /// <summary>
        /// 外積(self×a)を返す
        /// </summary>
        /// <param name="self"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float Cross(this Vector2 self, Vector2 a)
        {
            return self.x*a.x - self.y*a.y;
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

        /// <summary> xを差し替えたVector2返す。非破壊 </summary>
        public static Vector2 PutX(this Vector2 self, float x)
        {
           return new Vector2(x, self.y);
        }

        /// <summary> yを差し替えたVector2返す。非破壊 </summary>
        public static Vector2 PutY(this Vector2 self, float y)
        {
            return new Vector2(self.x, y);
        }
    }
}