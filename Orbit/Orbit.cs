using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniEx
{
    [System.Serializable]
    public abstract class Orbit2D
    {
        // 直進
        public static readonly Orbit2D Direct = new Orbit2DNoOption();

        // 法線方向を決める際のプライオリティ
        public enum NormPriority
        {
            Random, //ランダム
            Left, //法線が左を向くようにする
            Right, //右
            Up, //上
            Down, //下
            None, //指定しない
        }

        /// <summary>
        /// 優先度に応じて方向を変える
        /// </summary>
        protected static Vector2 AdjustWithPriority(Vector2 vec, NormPriority priority)
        {
            switch (priority)
            {
                case NormPriority.Left:
                    return vec.x > 0 ? -vec : vec;
                case NormPriority.Right:
                    return vec.x < 0 ? -vec : vec;
                case NormPriority.Up:
                    return vec.y < 0 ? -vec : vec;
                case NormPriority.Down:
                    return vec.y > 0 ? -vec : vec;
                default:
                    break;
            }

            return vec;
        }

        // 移動の原点となる場所
        protected virtual Vector2 OriginPos(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return edge.Start;
        }

        /// <summary>
        /// 計算実行(時間は線形)
        /// </summary>
        public virtual IEnumerable<Vector2> Calc(OrbitEdge edge, float time)
        {
            return Calc(edge, OrbitTimeFunc.Linear, time);
        }

        /// <summary>
        /// 計算実行
        /// </summary>
        public abstract IEnumerable<Vector2> Calc(OrbitEdge edge, OrbitTimeFunc timeFunc, float time);

        // baseIsRight : 角度0の時に, 右をむいているかどうか
        //public static IEnumerable<Pair<Vector2, float?>> CalculateWithRotate(IEnumerable<Vector2> data, bool baseIsRight)
        //{
        //	var offset = baseIsRight ?  0 : 180;
        //	Vector2 lastPos = Vector2.zero;
        //	foreach (var nextPos in data)
        //	{
        //		var delta = nextPos - lastPos;

        //		if (delta.sqrMagnitude < 1.0e-6)
        //			yield return new Pair<Vector2, float?>(nextPos, null);

        //		// 左向きを基準にするため180足してある			
        //		yield return new Pair<Vector2, float?>(nextPos, offset + Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
        //		lastPos = nextPos;
        //	}
        //}
    }

    /// <summary>
    /// オプション値有りバージョン.
    /// オプション値はCalculate実行時に最初に生成され, 接線,法線計算時の引数として渡される. 一回の軌道計算時にランダム性を持たせたい時などに用いる.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Orbit2D<T> : Orbit2D
    {
        // 法線のプライオリティ. 
        [SerializeField] NormPriority normalPriority_;

        public NormPriority NormalPriority
        {
            get { return normalPriority_; }
            set { normalPriority_ = value; }
        }

        protected abstract T CreateOption(OrbitEdge edge, OrbitTimeFunc timeFunc, float time);

        /// <summary>
        /// 接線方向の計算. デフォルトは直進. edge.Startにこの結果が接線方向として加えられる
        /// </summary>
        protected virtual Vector2 TangentFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime, T option)
        {
            return edge.GetDirection()*normalizedTime;
        }

        /// <summary>
        /// 法線方向の計算. デフォは0. edge.Startにこの結果が接線方向として加えられる
        /// </summary>
        protected virtual Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime, T option)
        {
            return Vector2.zero;
        }


        // 法線方向のベクトル計算(デフォルトではNormalFuncで計算した結果にPriorityによる調整を入れているのみ
        protected virtual Vector2 GetNormalVector(OrbitEdge edge, Vector2 lastPos, float normalizedTime, T option)
        {
            return AdjustWithPriority(NormalFunc(edge, lastPos, normalizedTime, option), NormalPriority);
        }

        // オプション有りバージョン
        protected virtual Vector2 GetTangentVector(OrbitEdge edge, Vector2 lastPos, float normalizedTime, T option)
        {
            return TangentFunc(edge, lastPos, normalizedTime, option);
        }

        public override IEnumerable<Vector2> Calc(OrbitEdge edge, OrbitTimeFunc timeFunc, float time)
        {
            float startTime = Time.time;
            float endTime = startTime + time;

            yield return edge.Start;

            var last = edge.Start;

            // 反転するかどうか
            var reverse = NormalPriority == NormPriority.Random && RandomUtility.CoinToss(50);

            // オプションを取得
            var option = CreateOption(edge, timeFunc, time);

            while (Time.time < endTime)
            {
                var p = timeFunc.TimeFunc((Time.time - startTime)/time);

                // 法線方向
                var tangent = GetTangentVector(edge, last, p, option);

                // 法線成分
                var normal = GetNormalVector(edge, last, p, option);

                if (reverse)
                    normal *= -1;

                // 保存
                last = OriginPos(edge, last, p) + tangent + normal;

                yield return last;
            }

            yield return edge.Dest;
        }
    }

    /// <summary>
    /// オプション値が必要ないOrbit
    /// </summary>
    public class Orbit2DNoOption : Orbit2D<object>
    {
        // オプション有りの方は継承できないようにする
        protected sealed override object CreateOption(OrbitEdge edge, OrbitTimeFunc timeFunc, float time)
        {
            return null;
        }

        // 接線方向の計算
        protected sealed override Vector2 TangentFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime,
            object option)
        {
            return TangentFunc(edge, lastPos, normalizedTime);
        }

        // 法線方向の計算
        protected sealed override Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime,
            object option)
        {
            return NormalFunc(edge, lastPos, normalizedTime);
        }

        // 法線方向のベクトル計算(デフォルトではNormalFuncで計算した結果にPriorityによる調整を入れているのみ
        protected sealed override Vector2 GetNormalVector(OrbitEdge edge, Vector2 lastPos, float normalizedTime,
            object option)
        {
            return GetNormalVector(edge, lastPos, normalizedTime);
        }

        // オプション有りバージョン
        protected sealed override Vector2 GetTangentVector(OrbitEdge edge, Vector2 lastPos, float normalizedTime,
            object option)
        {
            return GetTangentVector(edge, lastPos, normalizedTime);
        }



        // オプション無バージョン

        // デフォは親(自分自身だと無限再帰になるので注意)
        protected virtual Vector2 TangentFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return base.TangentFunc(edge, lastPos, normalizedTime, null);
        }

        // デフォは親(自分自身だと無限再帰になるので注意)
        protected virtual Vector2 NormalFunc(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return base.NormalFunc(edge, lastPos, normalizedTime, null);
        }


        // 法線方向のベクトル計算(デフォルトではNormalFuncで計算した結果にPriorityによる調整を入れているのみ
        protected virtual Vector2 GetNormalVector(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return base.GetNormalVector(edge, lastPos, normalizedTime, null);
        }

        protected virtual Vector2 GetTangentVector(OrbitEdge edge, Vector2 lastPos, float normalizedTime)
        {
            return base.GetTangentVector(edge, lastPos, normalizedTime, null);
        }
    }

}