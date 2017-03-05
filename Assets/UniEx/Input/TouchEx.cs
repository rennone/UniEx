using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// http://qiita.com/tempura/items/4a5482ff6247ec8873df
/// タッチとマウスの共通処理用
/// </summary>
public static class TouchEx
{
	/// <summary>
	/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
	/// </summary>
	public enum TouchPhase
	{
		None = -1,
		Began = 0,
		Moved = 1,
		Stationary = 2,
		Ended = 3,
		Canceled = 4
	}

	/// <summary>
	/// Androidフラグ
	/// </summary>
	static readonly bool IsAndroid = Application.platform == RuntimePlatform.Android;
	/// <summary>
	/// iOSフラグ
	/// </summary>
	static readonly bool IsIOS = Application.platform == RuntimePlatform.IPhonePlayer;
	/// <summary>
	/// エディタフラグ
	/// </summary> 
	static readonly bool IsEditor = !IsAndroid && !IsIOS;

	/// <summary>
	/// デルタポジション判定用・前回のポジション
	/// </summary>
	static Vector3 prebPosition_;

	/// <summary>
	/// タッチ情報を取得(エディタとスマホを考慮)
	/// </summary>
	/// <returns>タッチ情報</returns>
	public static TouchPhase GetPhase()
	{
		if (IsEditor)
		{
			if (Input.GetMouseButtonDown(0))
			{
				prebPosition_ = Input.mousePosition;
				return TouchPhase.Began;
			}
			else if (Input.GetMouseButton(0))
			{
				return TouchPhase.Moved;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				return TouchPhase.Ended;
			}
		}
		else
		{
			if (Input.touchCount > 0) return (TouchPhase)((int)Input.GetTouch(0).phase);
		}
		return TouchPhase.None;
	}
	/// <summary> タッチカウントを取得 </summary>
	public static int GetTouchCount()
	{
		if(IsEditor)
		{
			// 0 ~ 2 (左中右)を押しているか確認
			return Enumerable.Range(0, 3).Where(i => Input.GetMouseButton(i)).Count();
		}
		else
		{
			return Input.touchCount;
		}
	}

	/// <summary>
	/// タッチポジションを取得(エディタとスマホを考慮)
	/// </summary>
	/// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
	public static Vector3 GetPosition(int touchIndex = 0)
	{
		if (IsEditor)
		{
			if (GetPhase() != TouchPhase.None) return Input.mousePosition;
		}
		else
		{
			if (Input.touchCount > touchIndex) return Input.GetTouch(touchIndex).position;
		}
		return Vector3.zero;
	}
	

	/// <summary>
	/// タッチデルタポジションを取得(エディタとスマホを考慮)
	/// </summary>
	/// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
	public static Vector3 GetDeltaPosition()
	{
		if (IsEditor)
		{
			var phase = GetPhase();
			if (phase != TouchPhase.None)
			{
				var now = Input.mousePosition;
				var delta = now - prebPosition_;
				prebPosition_ = now;
				return delta;
			}
		}
		else
		{
			if (Input.touchCount > 0) return Input.GetTouch(0).deltaPosition;
		}
		return Vector3.zero;
	}
}

