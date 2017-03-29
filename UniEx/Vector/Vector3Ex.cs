using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniEx
{
    public static class Vector3Ex
    {
        /// <summary> xを差し替えたVector3返す。非破壊 </summary>
        public static Vector3 PutX(this Vector3 self, float x)
        {
            return new Vector3(x, self.y, self.z);
        }

        /// <summary> xyを差し替えたVector3返す。非破壊 </summary>
        public static Vector3 PutXy(this Vector3 self, float x, float y)
        {
            return new Vector3(x, y, self.z);
        }

        /// <summary> xzを差し替えたVector3返す。非破壊 </summary>
        public static Vector3 PutXz(this Vector3 self, float x, float z)
        {
            return new Vector3(x, self.y, z);
        }

        /// <summary> yを差し替えたVector3返す。非破壊 </summary>
        public static Vector3 PutY(this Vector3 self, float y)
        {
            return new Vector3(self.x, y, self.z);
        }

        /// <summary> yzを差し替えたVector3返す。非破壊 </summary>
        public static Vector3 PutYz(this Vector3 self, float y, float z)
        {
            return new Vector3(self.x, y, z);
        }

        /// <summary> zを差し替えたVector3返す。非破壊 </summary>
        public static Vector3 PutZ(this Vector3 self, float z)
        {
            return new Vector3(self.x, self.y, z);
        }
    }
}

