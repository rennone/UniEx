using System;
using UnityEngine;
using System.Runtime.InteropServices;
namespace UniEx
{
    [StructLayout(LayoutKind.Sequential)]
    [System.Serializable]
    public struct Location
    {

        // radiun, degree変換
        public const double dRad2Deg = 180.0/Math.PI;
        public const double dDeg2Rad = Math.PI/180.0;

        /// <summary>
        /// null代わりの値. 日本にいる限り負数はあり得ない.
        /// </summary>
        public static readonly Location null_ = new Location(-100, -100);

        /// <summary>
        /// 地球半径(m)
        /// </summary>
        public readonly static double radius_ = 6378137;

        /// <summary>
        /// 地球円周(m)
        /// </summary>
        public readonly static double round_ = 2*Math.PI*radius_;

        // 地球は球

        /// <summary>
        /// あるロケーションからあるロケーションまでの距離
        /// </summary>
        public static double Distance(Location lhs, Location rhs)
        {
            // TODO
            var lhsLatitude = lhs.Latitude2Rad;
            var lhsLongitude = lhs.Longitude2Rad;
            var rhsLatitude = rhs.Latitude2Rad;
            var rhsLongitude = rhs.Longitude2Rad;
            return radius_*
                   Math.Acos(Math.Sin(lhsLatitude)*Math.Sin(rhsLatitude) +
                             Math.Cos(lhsLatitude)*Math.Cos(rhsLatitude)*Math.Cos(rhsLongitude - lhsLongitude));
        }

        /// <summary>
        /// あるロケーションからあるロケーションまでの方向
        /// </summary>
        public static double Direction(Location from, Location to)
        {
            // TODO
            var fromLongitude = from.Latitude2Rad;
            var fromLatitude = from.Longitude2Rad;
            var toLongitude = to.Latitude2Rad;
            var toLatitude = to.Longitude2Rad;
            var deltaLatitude = toLatitude - fromLatitude;
            var degree = 90 -
                         Math.Atan2(Math.Sin(deltaLatitude),
                             Math.Cos(fromLongitude)*Math.Tan(toLongitude) -
                             Math.Sin(fromLongitude)*Math.Cos(deltaLatitude))/Math.PI*180;
            if (degree <= 0) degree += 360;
            if (360 <= degree) degree %= 360;
            return degree;
        }

        /// <summary>
        /// 緯度 度数 y軸に対応
        /// </summary>
        public double latitude_;

        /// <summary>
        /// 経度 度数 x軸に対応
        /// </summary>
        public double longitude_;

        /// <summary>
        /// 緯度のラジアン
        /// </summary>
        public double Latitude2Rad
        {
            get { return latitude_*dDeg2Rad; }
        }

        /// <summary>
        /// 経度のラジアン
        /// </summary>
        public double Longitude2Rad
        {
            get { return longitude_*dDeg2Rad; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Location(double latitude, double longitude)
        {
            latitude_ = latitude;
            longitude_ = longitude;
        }

        /// <summary>
        /// このロケーションを中心とした一辺sideMeterの正方形左上右下の頂点をかえす
        /// </summary>
        public Location[] GetSquareFromSideMeter(double sideMeter)
        {
            return GetRectFromSideMeter(sideMeter, sideMeter);
        }

        /// <summary>
        /// このロケーションを中心とした一辺sideMeterの長方形左上右下の頂点をかえす
        /// </summary>
        public Location[] GetRectFromSideMeter(double latiSide, double longSide)
        {
            var unitLati = round_/360;
            var halfLati = (latiSide*0.5)/unitLati;
            var unitLongi = round_*Math.Cos(Latitude2Rad)/360;
            var halfLongi = (longSide*0.5)/unitLongi;

            return new Location[]
            {
                new Location(latitude_ + halfLati, longitude_ - halfLongi),
                new Location(latitude_ - halfLati, longitude_ + halfLongi)
            };
        }

        // 現在位置における. meter[m]の緯度変換
        public double LatitudeByMeter(double meter)
        {
            var unitLati = round_/360;
            return meter/unitLati;
        }

        // 現在位置におけるmeter[m]の経度変換
        public double LongitudeByMeter(double meter)
        {
            var unitLongi = round_*Math.Cos(Latitude2Rad)/360;
            return meter/unitLongi;
        }

        // 現在位置から緯度latitude(経度は同じ)の距離をメートルで取得
        // 引数の緯度が高くなる(北に行く)ほど大きくなる
        public double MeterByLattitudeTo(double latitude)
        {
            // 縦方向の差分を求める.
            var r = dDeg2Rad*radius_;
            var arc1 = r*latitude_;
            var arc2 = r*latitude;
            return arc2 - arc1;
        }

        // 現在位置から経度longitude(緯度は同じ)の距離をメートルで取得
        // 引数の経度が大きくなる(東に行く)ほど大きくなる
        public double MeterByLongitudeTo(double longitude)
        {
            var r = dDeg2Rad*radius_*Math.Cos(Latitude2Rad);
            var arc1 = r*longitude_;
            var arc2 = r*longitude;
            return arc2 - arc1;
        }

        /// <summary>
        ///  MeterByLatitude, MeterByLongitudeをまとめたもの
        ///  locationまでのベクトルをメートル単位で取得
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        //public Point<double> MeterByLocationTo(Location location)
        //{
        //    return new Point<double>(MeterByLongitudeTo(location.longitude_), MeterByLattitudeTo(location.latitude_));
        //}

        public Location MovedMeterLocation(Vector2 meter)
        {
            return new Location(latitude_ + LatitudeByMeter(meter.y), longitude_ + LongitudeByMeter(meter.x));
        }

        public override string ToString()
        {
            return string.Format("Lati{0} Longi{1}", latitude_, longitude_);
        }

        /// <summary>
        /// Vector2変換
        /// Note : double -> floatによる情報落ちが発生する
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2()
        {
            return new Vector2((float) longitude_, (float) latitude_);
        }

        // Locationの矩形（左上右下の配列）の範囲内かを判定
        public bool IsCollide(double left, double bottom, double right, double top)
        {
            return (left <= longitude_ && longitude_ <= right &&
                    bottom <= latitude_ && latitude_ <= top);
        }

        // +演算子
        public static Location operator +(Location a, Location b)
        {
            return new Location(a.latitude_ + b.latitude_, a.longitude_ + b.longitude_);
        }

        // -演算子
        public static Location operator -(Location a, Location b)
        {
            return new Location(a.latitude_ - b.latitude_, a.longitude_ - b.longitude_);
        }

        public static Location operator *(Location a, float t)
        {
            return new Location(a.latitude_*t, a.longitude_*t);
        }

        public static Location operator *(float t, Location a)
        {
            return a*t;
        }
    }

    public struct LocationRect
    {
        public double Left;
        public double Right;
        public double Top;
        public double Bottom;

        LocationRect(Location pos, Location size)
        {
            Left = pos.longitude_;
            Right = pos.longitude_ + size.longitude_;
            Top = pos.latitude_;
            Bottom = pos.latitude_ + size.latitude_;

        }

        LocationRect(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public bool Overlaps(LocationRect other)
        {
            return (Left <= other.Right && other.Left <= Right &&
                    Top <= other.Bottom && other.Top <= Bottom);

        }
    }
}