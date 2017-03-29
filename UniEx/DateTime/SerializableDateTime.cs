using UnityEngine;
using System;
using CsEx;

namespace UniEx
{
    /// <summary>
    ///  DateTimeはScriptableObjectでシリアライズできないので,
    ///  文字列を保存して実行時に変換するようにするためのクラス
    /// </summary>
    [System.Serializable]
    public struct SerializableDateTime
    {
        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        [SerializeField] private readonly string dateStr_;

        // DateTimeはNull非許容型なのでNullrableで持つ
        DateTime? dateTime_;

        public DateTime DateTime
        {
            get
            {
                if (dateTime_.HasValue == false)
                {
                    // 空の場合デフォルト値を返す
                    if (IsNull)
                        return default(DateTime);

                    dateTime_ = DateTime.Parse(dateStr_);
                }

                return dateTime_.Value;
            }
        }

        // 日付が空かどうか
        public bool IsNull
        {
            get { return dateStr_.IsNullOrEmpty(); }
        }


        public SerializableDateTime(string str)
        {
            if (str.IsNullOrEmpty())
                str = DateTime.MinValue.ToString(DateFormat);

            dateStr_ = str;

            DateTime dateTime;
            // パース出来るかチェックする
            if (System.DateTime.TryParse(dateStr_, out dateTime))
            {
                dateTime_ = dateTime;
                // 空白とかのゴミが入っていた場合正しいフォーマット文字列に戻す
                dateStr_ = dateTime_.Value.ToString(DateFormat);
            }
            else
            {
                dateTime_ = null;
            }
        }

        public SerializableDateTime(DateTime dateTime)
        {
            dateStr_ = dateTime.ToString(DateFormat);
            dateTime_ = dateTime;
        }

        public override string ToString()
        {
            return dateStr_;
        }

        // メンバのDateTimeがnullかどうかでイコールチェックできるようにする
        public override bool Equals(object obj)
        {
            // 
            return obj == null ? IsNull : base.Equals(obj);
        }

        public bool Equals(SerializableDateTime d)
        {
            // 日付が同じかチェック
            return !d.IsNull && d.DateTime == DateTime;
        }

        public static bool operator ==(SerializableDateTime lhs, SerializableDateTime rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SerializableDateTime lhs, SerializableDateTime rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return dateStr_.GetHashCode();
        }
    }
}