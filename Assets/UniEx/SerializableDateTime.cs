using UnityEngine;
using System.Collections;
using System;

/// <summary>
///  DateTimeはScriptableObjectでシリアライズできないので,
///  文字列を保存して実行時に変換するようにするためのクラス
/// </summary>
[System.Serializable]
public struct SerializableDateTime
{
	public const string DateFormat = "yyyy-MM-dd HH:mm:ss";

	[SerializeField]
	string dateStr_;

	// DateTimeはNull非許容型なのでNullrableで持つ
	DateTime? dateTime_;
	public DateTime DateTime
	{
		get
		{
			if (dateTime_.HasValue == false)
			{
				// 空の場合デフォルト値を返す
				if( IsNull )
					return default(DateTime);
				
				dateTime_ = DateTime.Parse(dateStr_);
			}

			return dateTime_.Value;
		}
	}

	public bool IsNull { get { return dateStr_.IsNullOrEmpty(); } }

	// 日付が空かどうか
	public bool IsValid { get { return !IsNull; } }
	
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
		if(obj == null)
		{
			// DateTimeがnullかどうか
			return IsNull;
		}

		return base.Equals(obj);
	}

	public bool Equals(SerializableDateTime d)
	{
		// 日付が同じかチェック
		return d.IsValid && d.DateTime == DateTime;
	}

	public static bool operator ==(SerializableDateTime lhs, SerializableDateTime rhs)
	{

		// lhsがnullでない場合, Equal関数を呼んでチェック
		if ((object)lhs != null)
			return lhs.Equals(rhs);

		// ここに来た段階で
		// lhs = null かつ rhs != null
		return rhs.IsNull;
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