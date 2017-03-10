using System;
using System.Collections.Generic;
using UniEx.Operator;
using UnityEngine;

namespace UniEx
{
    public struct Point3<T> : IEquatable<Point3<T>>
    {
       

        public T x_;
        public T y_;
        public T z_;

        public Point3(T x, T y, T z)
        {
            x_ = x;
            y_ = y;
            z_ = z;
        }

        public Point3(T x, T y)
        {
            x_ = x;
            y_ = y;
            z_ = default(T);
        }

        private static T Add(T a, T b)
        {
            return Operator<T>.Add(a, b);
        }

        private static T Subtract(T a, T b)
        {
            return Operator<T>.Subtract(a, b);
        }

        private static T Multiply(T a, T k)
        {
            return Operator<T>.Multiply(a, k);
        }

        private static T Divide(T a, T k)
        {
            return Operator<T>.Divide(a, k);
        }

        private static T Negate(T a)
        {
            return Operator<T>.Negate(a);
        }

        private static double ToDouble(T a)
        {
            return Operator<T>.ToDouble(a);
        }

        private static float ToFloat(T a)
        {
            return Operator<T>.ToFloat(a);
        }

        private static int ToInt(T a)
        {
            return Operator<T>.ToInt(a);
        }

        /// <summary>
        /// 足し算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point3<T> operator+(Point3<T> a, Point3<T> b)
        {
            return new Point3<T>(Add(a.x_, b.x_), Add(a.y_, b.y_), Add(a.z_, b.z_));
        }

        /// <summary>
        /// 引き算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point3<T> operator -(Point3<T> a, Point3<T> b)
        {
            return new Point3<T>(Subtract(a.x_, b.x_), Subtract(a.y_, b.y_), Subtract(a.z_, b.z_));
        }

        /// <summary>
        /// 負数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Point3<T> operator -(Point3<T> a)
        {
            return new Point3<T>(Negate(a.x_), Negate(a.y_), Negate(a.z_));
        }

        /// <summary>
        /// 掛け算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Point3<T> operator *(Point3<T> a, T k)
        {
            return new Point3<T>(Multiply(a.x_, k), Multiply(a.y_, k), Multiply(a.z_, k));
        }

        /// <summary>
        /// 割り算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Point3<T> operator /(Point3<T> a, T k)
        {
            return new Point3<T>(Divide(a.x_, k), Divide(a.y_, k), Divide(a.z_, k));
        }

        public static bool operator ==(Point3<T> a, Point3<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point3<T> a, Point3<T> b)
        {
            return !(a == b);
        }

        public static implicit operator Point2<T>(Point3<T> a)
        {
            return new Point2<T>(a.x_, a.y_);
        }

        public static implicit operator Vector3(Point3<T> a)
        {
            return a.Vec3;
        }

        public Point3<float> Float
        {
            get { return new Point3<float>(ToFloat(x_), ToFloat(y_), ToFloat(z_)); }
        }

        public Point3<double> Double
        {
            get { return new Point3<double>(ToDouble(x_), ToDouble(y_), ToDouble(z_)); }
        }

        public Point3<int> Int
        {
            get { return new Point3<int>(ToInt(x_), ToInt(y_), ToInt(z_)); }
        }

        public Vector3 Vec3
        {
            get { return new Vector3(ToFloat(x_), ToFloat(y_), ToFloat(z_)); }
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x_, y_, z_);
        }

        public bool Equals(Point3<T> other)
        {
            return x_.Equals(other.x_) && y_.Equals(other.y_) && z_.Equals(other.z_);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point3<T> && Equals((Point3<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(x_);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(y_);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(z_);
                return hashCode;
            }
        }
    }
}
