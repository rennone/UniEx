using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

/// タッチ判定は透過させ, かつ範囲をタップしたことを検出させるためのコンポーネント
public partial class IgnoreRaycaster : MonoBehaviour, ICanvasRaycastFilter
{
	[Serializable]
	public class MouseUpEvent : UnityEvent { }

	[SerializeField]
	MouseUpEvent onMouseUp_;

	public MouseUpEvent OnMouseUp { get { return onMouseUp_; } }


	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		// マウスアップを検知
		if(Input.GetMouseButtonUp(0) && onMouseUp_ != null)
		{
			OnMouseUp.Invoke();
		}

		return false;
	}
}
