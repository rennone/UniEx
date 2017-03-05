using System;

namespace UniEx
{
    public class ThreadSafeRandom
    {
        private static readonly System.Random global_ = new System.Random();

        // 各スレッドごとに独立して作られる
        [ThreadStatic]
        private static System.Random local_;

        static System.Random Local
        {
            get
            {
                if (local_ == null)
                {
                    int seed;
                    lock (global_)
                    {
                        seed = global_.Next();
                    }

                    local_ = new System.Random(seed);
                }

                return local_;
            }
        }

        public static int Next()
        {
            return Local.Next();
        }


        public static int Next(int min, int max)
        {
            return Local.Next(min, max);
        }

        public static double NextDouble() { return Local.NextDouble(); }
    }

}
