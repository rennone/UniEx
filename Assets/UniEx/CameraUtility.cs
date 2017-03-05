using UnityEngine;
using System.Collections;

public class CameraUtility
{
	// 画像のサイズ
	static public float Width { get { return 640.0f; } }
	static public float Height { get { return 1136.0f; } }
	
	// TODO : Editor上で画面をリアルタイムに変更したときに対応するように毎回計算しているが
	// 実体の端末では画面サイズは固定なのでDEBUGマクロなどで切り返るようにすべき
	// 端末が上記のアスペクト比より
	// 横長 : GameScreenWidth  < Screen.Width
	// 縦長 : GameScreenHeight < Screen.Height
	static public float GameScreenWidth { get { return Width * GetViewportScale(); } }
	static public float GameScreenHeight { get { return Height * GetViewportScale(); } }
	static public Vector2 GameScreenSize { get { return new Vector2(GameScreenWidth, GameScreenHeight); } }

	static public float GameScreenLeft { get { return (Screen.width - GameScreenWidth) / 2; } }
	static public float GameScreenRight { get { return (Screen.width + GameScreenWidth) / 2; } }

	static public float GameScreenTop { get { return (Screen.height + GameScreenHeight) / 2; } }
	static public float GameScreenBottom { get { return (Screen.height - GameScreenHeight) / 2; } }

	static public Vector2 GameScreenLB { get { return new Vector2(GameScreenLeft, GameScreenBottom); } }
	static public Vector2 GameScreenRT { get { return new Vector2(GameScreenRight, GameScreenTop); } }

	// スクリーン座標をワールド座標に変換（解像度変化に対応）
	static public Vector2 ScreenToWorldPoint(Camera camera, Vector2 screenPos)
	{ 
		// TODO 計算の無駄が多い...
		float scale = GetViewportScale();
		Vector2 rate = GetViewportRate();
        Vector2 offset = new Vector2(Screen.width * (1 - rate.x)/2, Screen.height * (1 - rate.y) / 2);
		return camera.ScreenToWorldPoint(new Vector2(offset.x + screenPos.x * scale, offset.y + screenPos.y * scale));
	}

	// ワールド座標をスクリーン座標に変換（解像度変化に対応）
	static public Vector2 WorldToScreenPoint(Camera camera, Vector2 worldPos)
	{ 
		float scale = GetViewportScale();
		Vector2 rate = GetViewportRate();
        Vector2 offset = new Vector2(Screen.width * (1 - rate.x) / 2, Screen.height * (1 - rate.y) / 2);

		
		var pos = RectTransformUtility.WorldToScreenPoint(camera, new Vector3(worldPos.x, worldPos.y));

		return new Vector2((pos.x - offset.x ) / scale, (pos.y - offset.y) / scale);
	}

	// 現在のアスペクト比からのビューポートの比率を取得
	static public Vector2 GetViewportRate()
	{
		float scale = GetViewportScale();
 		return new Vector2(Width * scale / Screen.width, Height * scale / Screen.height);
	}

	// 現在の画面サイズにおけるビューポートの拡縮比率を取得
	static public float GetViewportScale()
	{ 
       // 縦横のスケールを計算
        Vector2 rate = new Vector2(Screen.width / Width, Screen.height / Height);

        // 小さいほうに合わせる
        return rate.x < rate.y ? rate.x : rate.y;
	}

	/// <summary> 画面サイズがwidth, heightの時にカメラのscaleを計算する </summary>
	public static float GetViewportScale(float width, float height)
	{
		// 縦横のスケールを計算
		Vector2 rate = new Vector2(width / Width, height / Height);

		// 小さいほうに合わせる
		return rate.x < rate.y ? rate.x : rate.y;
	}
}
