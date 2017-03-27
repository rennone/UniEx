namespace UniEx
{
    public static class MathEx
    {
        /// <summary>
        /// 可変長引数のMax関数(double)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Max(params double[] x)
        {
            return x.FindMax(a => a);
        }

        /// <summary>
        /// 可変長引数のMin関数(double)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Min(params double[] x)
        {
            return x.FindMin(a => a);
        }

        /// <summary>
        /// 可変長引数のMax関数(double)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Max(params float[] x)
        {
            return x.FindMax(a => a);
        }

        /// <summary>
        /// 可変長引数のMin関数(double)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Min(params float[] x)
        {
            return x.FindMin(a => a);
        }

        /// <summary>
        /// 可変長引数のMax関数(double)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Max(params int[] x)
        {
            return x.FindMax(a => a);
        }

        /// <summary>
        /// 可変長引数のMin関数(double)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Min(params int[] x)
        {
            return x.FindMin(a => a);
        }
    }
}