using CsEx;
using UnityEngine;

namespace UniEx
{
    public static class MathfEx
    {
        public static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 t)
        {
            return Lerp(a, b, t.x, t.y);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float xt, float yt)
        {
            return new Vector2(Mathf.Lerp(a.x, b.x, xt), Mathf.Lerp(a.y, b.y, yt));
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return Lerp(a, b, t, t);
        }

        /// <summary> 引数の10進数における桁数を取得 </summary>
        public static int GetDigit(int num)
        {
            return (num == 0) ? 1 : (int) Mathf.Log10(num) + 1;
        }

        /// <summary>引数の10進数における桁数を取得(long型)</summary>
        public static int GetDigit(long num)
        {
            return num == 0 ? 1 : (int) Mathf.Log10(num) + 1;
        }

        /// <summary>
        /// 正規化した(a - min) / (max - min) を0~1でClampした結果を返す
        /// </summary>
        public static float NormalizedClamp(float a, float min, float max)
        {
            return Mathf.Clamp((a - min)/(max - min), 0, 1);
        }

        /// <summary> 可変長引数のMathf.Min </summary>
        public static float Min(params float[] x)
        {
            return x.FindMin(a => a);
        }

        /// <summary> 可変長引数のMathf.Max </summary>
        public static float Max(params float[] x)
        {
            return x.FindMax(a => a);
        }

        /// <summary> 可変長引数のMathf.Min </summary>
        public static int Min(params int[] x)
        {
            return x.FindMin(a => a);
        }

        /// <summary> 可変長引数のMathf.Max </summary>
        public static int Max(params int[] x)
        {
            return x.FindMax(a => a);
        }

        /// <summary> 要素ごとのMin結果を取得 </summary>
        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
        }

        /// <summary> 要素ごとのMin結果を取得 </summary>
        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
        }
    }
}