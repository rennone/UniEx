using UnityEngine;
using UnityEngine.UI;

namespace UniEx
{
    /// <summary>
    /// 複数のCanvasが共存する場合に, GraphicsRaycasterのプライオリティを適切に設定しないと後ろのボタンが反応してしまうといったことが発生する。
    /// 通常のGraphicRaycasterはタッチの反応のプライオリティをInspectorで設定することが出来ないのでそれ用
    /// </summary>
    [System.Serializable]
    public class CustomGraphicRaycaster : GraphicRaycaster
    {
        // Inspectorで設定する優先度
        [SerializeField] int priority_;

        public override int sortOrderPriority
        {
            get { return priority_; }
        }
    }
}