using System;

namespace UniEx
{
    /// <summary> Enum拡張 </summary> 
    public static class EnumEx
    {

        /// <summary> Enum値の総数を返す </summary> 
        public static int Num<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }
    }
}

