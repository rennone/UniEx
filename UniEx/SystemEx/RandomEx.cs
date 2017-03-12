using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace UniEx
{
    public static class RandomUtility
    {
        // ランダムなVector2を返す
        public static Vector2 RandomVector(float min, float max)
        {
            var x = Range(min, max);
            var y = Range(min, max);

            return new Vector2(x, y);
        }



        // ランダムなVector2を返す
        public static Vector2 RandomVector(Vector2 min, Vector2 max)
        {
            var x = Range(min.x, max.x);
            var y = Range(min.y, max.y);

            return new Vector2(x, y);
        }


        //public static Location RandomLocation(Location min, Location max)
        //{
        //	var lat = NextDouble() * (max.latitude_ - min.latitude_) + min.latitude_;
        //	var lon = NextDouble() * (max.longitude_ - min.longitude_) + min.longitude_;
        //	return new Location(lat, lon);
        //}

        //public static Location ThreadSafeRandomLocation(Location min, Location max)
        //{
        //	var lat = ThreadSafeNextDouble() * (max.latitude_ - min.latitude_) + min.latitude_;
        //	var lon = ThreadSafeNextDouble() * (max.longitude_ - min.longitude_) + min.longitude_;
        //	return new Location(lat, lon);
        //}

        // successRate[%]の確率でtrueを返す
        public static bool CoinToss(int successRatePercent)
        {
            return Range(0, 100) < successRatePercent;
        }

 

        public static bool CoinToss(float successRatePercent)
        {
            // Randomのvalueが1.0を含んでしまうので
            // 100%かどうかはチェックを入れている
            // 等号だと誤差により, 100%で失敗する可能性があるかもしれないので
            // 100 - 1e-6 [%]以上が 100%扱いになる. 
            if (successRatePercent > 100f - Mathf.Epsilon)
            {
                return true;
            }

            return Range(0f, 100f) < successRatePercent;
        }

        // n面のサイコロを振った時の結果を 0 ~ n-1 で返す
        // 各面の重み(と数)はweightsで指定する
        public static int DiceToss(params int[] weights)
        {
            return DiceToss(weights.AsEnumerable());
        }

        public static int DiceToss(IEnumerable<int> weights)
        {
            // 重みの全合計値から乱数発生
            // 0 ~ sum-1
            var value = Range(0, weights.Sum());

            int total = 0;
            foreach (var item in weights.Select((v, i) => new { v, i }))
            {
                // それまでの合計値
                total += item.v;

                // 超えた瞬間その目が答え
                if (total > value)
                    return item.i;
            }

            // パラメータが無いときに来る
            //DebugLogger.LogWarning("DiceToss : no weight parameter", DebugLogger.Tag.Global);
            return weights.Count() - 1;
        }



        /// <summary>
        /// DiceTossをnum回振った時の結果を取得する
        /// ret[j] : weights[j]で表される面が出た回数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static int[] DiceToss(int num, params int[] weights)
        {
            if (num <= 0 || weights.Length == 0)
                return new int[0];

            int[] ret = new int[weights.Length];

            for (int i = 0; i < num; ++i)
                ret[DiceToss(weights)] += 1;

            return ret;
        }



        /// <summary>
        ///  return [inclusive]min ~ max[exclusive]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }

        public static int ThreadSafeRange(int min, int max)
        {
            return SafeRandom.Range(min, max);
        }

        /// <summary>
        ///  return [inclusive]min ~ max[iclusive]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }
        
        public static double NextDouble()
        {
            return Random.value;
        }
    }
}
