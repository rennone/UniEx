using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniEx
{
    public static class Color32Ex
    {

        public static readonly Color32 black_ = new Color32(0, 0, 0, 255);
        public static readonly Color32 white_ = new Color32(255, 255, 255, 255);
        public static readonly Color32 red_ = new Color32(255, 0, 0, 255);
        public static readonly Color32 green_ = new Color32(0, 255, 0, 255);
        public static readonly Color32 blue_ = new Color32(0, 0, 255, 255);

        /// <summary> α値を除いて一致するか </summary>
        public static bool Equal24(this Color32 self, Color32 other)
        {
            return self.r == other.r && self.g == other.g && self.b == other.b;
        }

        public static bool Equal24(this Color32 self, Color32 other, int acceptError)
        {
            if (acceptError == 0)
                return self.Equal24(other);

            return
                self.r >= other.r - acceptError &&
                self.r <= other.r + acceptError &&
                self.g >= other.g - acceptError &&
                self.g <= other.g + acceptError &&
                self.b >= other.b - acceptError &&
                self.b <= other.b + acceptError;
        }

        /// <summary> α値込みで一致するか </summary>
        public static bool Equal32(this Color32 self, Color32 other)
        {
            return self.Equal24(other) && self.a == other.a;
        }

        public static bool Equal32(this Color32 self, Color32 other, int acceptError)
        {
            return
                self.Equal24(other) &&
                self.a >= other.a - acceptError &&
                self.a <= other.a + acceptError;
        }
    }

}