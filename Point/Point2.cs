using System;
using System.Collections.Generic;
using UniEx.Operator;
using UnityEngine;

namespace UniEx
{
    public struct Point2<T> : IEquatable<Point2<T>>
    {
       
        public T x_;
        public T y_;

        public Point2(T x, T y)
        {
            x_ = x;
            y_ = y;
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
        public static Point2<T> operator+(Point2<T> a, Point2<T> b)
        {
            return new Point2<T>(Add(a.x_,b.x_), Add(a.y_,b.y_));
        }

        /// <summary>
        /// 引き算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point2<T> operator -(Point2<T> a, Point2<T> b)
        {
            return new Point2<T>(Subtract(a.x_, b.x_), Subtract(a.y_, b.y_));
        }

        /// <summary>
        /// 負数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Point2<T> operator -(Point2<T> a)
        {
            return new Point2<T>(Negate(a.x_), Negate(a.y_));
        }

        /// <summary>
        /// 掛け算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Point2<T> operator *(Point2<T> a, T k)
        {
            return new Point2<T>(Multiply(a.x_, k), Multiply(a.y_, k));
        }

        /// <summary>
        /// 割り算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Point2<T> operator /(Point2<T> a, T k)
        {
            return new Point2<T>(Divide(a.x_, k), Divide(a.y_, k));
        }

        public static bool operator ==(Point2<T> a, Point2<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point2<T> a, Point2<T> b)
        {
            return !(a == b);
        }

        public static implicit operator Point3<T>(Point2<T> a)
        {
            return new Point3<T>(a.x_, a.y_);
        }

        public static implicit operator Vector2(Point2<T> a)
        {
            return a.Vec2;
        }

        public Point2<float> Float
        {
           get{ return new Point2<float>(ToFloat(x_), ToFloat(y_));}
        }

        public Point2<double> Double
        {
            get { return new Point2<double>(ToDouble(x_), ToDouble(y_)); }
        }

        public Point2<int> Int
        {
            get { return new Point2<int>(ToInt(x_), ToInt(y_)); }
        }

        public Vector2 Vec2
        {
            get { return new Vector2(ToFloat(x_), ToFloat(y_)); }
        }

        public override string ToString()
        {
            return string.Format("({0},{1}", x_, y_);
        }

        public bool Equals(Point2<T> other)
        {
            return x_.Equals(other.x_) && y_.Equals(other.y_);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point2<T> && Equals((Point2<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(x_) * 397) ^ EqualityComparer<T>.Default.GetHashCode(y_);
            }
        }

    }


}
