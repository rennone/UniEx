using UnityEngine;
using System.Collections;

public static class RectTransformEx
{
	// RectTransformの端点のWorld座標をRectで表現
	public static Rect GetWorldRect(this RectTransform rt)
	{
		// コーナーを取得
		// [0] : 左下( x min, y min)
		// [1] : 左上( x min, y max)
		// [2] : 右上( x max, y max)
		// [3] : 右下( x max, y min)
		Vector3[] corner = new Vector3[4];
		 rt.GetWorldCorners(corner);


		var size = corner[2] - corner[0];
		return new Rect(corner[0], size);
	}

	/// <summary>
	/// 拡張機能：anchoredPosition.xを設定する
	/// </summary>
	public static void SetAnchorPosX(this RectTransform rt, float x) { rt.anchoredPosition  = new Vector2(x, rt.anchoredPosition.y); }

	/// <summary>
	/// 拡張機能：anchoredPosition.yを設定する
	/// </summary>
	public static void SetAnchorPosY(this RectTransform rt, float y) { rt.anchoredPosition  = new Vector2(rt.anchoredPosition.x, y); }

}
