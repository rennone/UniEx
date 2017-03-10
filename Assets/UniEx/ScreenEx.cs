using UnityEngine;
using System.Collections;


namespace UniEx
{
    // 端末が上記のアスペクト比より
    // 横長 : GameScreenWidth  < Screen.Width
    // 縦長 : GameScreenHeight < Screen.Height
    public class ScreenEx
    {
        public static Vector2 ScreenSize { get { return new Vector2(Screen.width, Screen.height);} }

        // 画像のサイズ
        public float BaseWidth { get { return BaseSize.x; }}

        public float BaseHeight { get { return BaseSize.y; } }

        public Vector2 BaseSize { get; }

        public float ViewportScale { get; }

        public Rect TargetRect { get; }

        public Vector2 TargetSize { get { return TargetRect.size; } }

        public ScreenEx(float width, float height)
        {
            BaseSize = new Vector2(width, height);
            ViewportScale = Mathf.Min(Screen.width / width, Screen.height / height);

            var size = BaseSize*ViewportScale;
            TargetRect = new Rect((ScreenSize - size) / 2, size);
        }

        // スクリーン座標をワールド座標に変換（解像度変化に対応）
        public Vector2 BaseScreen2WorldPoint(Camera camera, Vector2 screenPos)
        {
            return camera.ScreenToWorldPoint( TargetRect.min + screenPos * ViewportScale);
        }

        // ワールド座標をスクリーン座標に変換（解像度変化に対応）
        public Vector2 World2BaseScreenPoint(Camera camera, Vector2 worldPos)
        {
            var pos = RectTransformUtility.WorldToScreenPoint(camera, new Vector3(worldPos.x, worldPos.y));
            return (pos - TargetRect.min) / ViewportScale;
        }

        /// <summary> 画面サイズがwidth, heightの時にカメラのscaleを計算する </summary>
        public float GetViewportScale(float width, float height)
        {
            return Mathf.Min(width/BaseWidth, height/BaseHeight);
        }
    }
}