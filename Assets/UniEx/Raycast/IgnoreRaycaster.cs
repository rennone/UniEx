using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

namespace UniEx
{
    /// タッチ判定は透過させ, かつ範囲をタップしたことを検出させるためのコンポーネント
    public class IgnoreRaycaster : MonoBehaviour, ICanvasRaycastFilter
    {
        [Serializable]
        public class MouseUpEvent : UnityEvent{}

        [SerializeField]
        MouseUpEvent onMouseUp_;

        /// <summary>
        /// MouseUp時のイベント
        /// </summary>
        public MouseUpEvent OnMouseUp
        {
            get { return onMouseUp_; }
            set { onMouseUp_ = value; }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            // マウスアップを検知
            if (Input.GetMouseButtonUp(0) && onMouseUp_ != null)
            {
                OnMouseUp.Invoke();
            }

            // タッチ判定を透過させるために常にfalseを返す
            return false;
        }
    }
}