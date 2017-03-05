using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace UniEx
{
    public class SpriteButtonSystem
    {
        // 押したオブジェクト
        Sprite2DButton downedBtn_ = null;

        // マウスがうえにあるオブジェクト
        Sprite2DButton pushedBtn_ = null;

        // タップした位置
        static Vector3 ScreenPoint
        {
            get { return Input.mousePosition; }
        }

        void Check(Collider2D collider)
        {
            Sprite2DButton btn = null;

            // GUIが手前にあるかチェック
            collider = UGUIPassCheck(collider);

            // 衝突発生
            if (collider)
            {
                // ボタン取得
                btn = collider.gameObject.GetComponentInChildren<Sprite2DButton>();
                if (btn != null)
                {
                    // 前のボタンと違う場合, Enter命令
                    if (btn != pushedBtn_)
                        btn.OnPointerEnter();
                }
            }

            // pushedBtn_の上から外れた場合, Exit命令
            if (pushedBtn_ != btn && pushedBtn_ != null)
                pushedBtn_.OnPointerExit();

            // 押し込んだ瞬間のオブジェクトを取得
            if (Input.GetMouseButtonDown(0))
                downedBtn_ = btn;

            // 押したオブジェクトの上で離したらClick命令
            if (Input.GetMouseButtonUp(0))
            {
                if (btn == downedBtn_ && btn != null)
                    btn.OnPointerClick();

                downedBtn_ = null;
            }

            pushedBtn_ = btn;
        }

        Vector3 GetMouseWorldPoint(Camera camera)
        {
            return camera.ScreenToWorldPoint(ScreenPoint);
        }

        public void Update(Camera camera)
        {
            // マウスの位置が重なるボタンを取得
            Check(Physics2D.OverlapPoint(GetMouseWorldPoint(camera)));
        }

        public void Update(Camera camera, int layerMask)
        {
            // マウスの位置が重なるボタンを取得
            Check(Physics2D.OverlapPoint(GetMouseWorldPoint(camera), layerMask));
        }

        Collider2D UGUIPassCheck(Collider2D collider)
        {
            // 最初からcolliderがnullの場合は無駄にチェックせずにnullを返す
            if (collider == null)
                return collider;

            // uGUIと被っているかチェック

            // EventSystemでレイを飛ばしてチェック
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = ScreenPoint;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            // colliderが示すgameObjectの手前にあるuGUIがあればnullを返す
            // TODO : Cameraのオーダーによってはz値で比べるのはできないかもしれない
            if (results.Any(m => m.gameObject.transform.position.z < collider.gameObject.transform.position.z))
            {
                return null;
            }

            return collider;
        }

        // 履歴のクリア
        public void Clear()
        {
            // Enter命令を出したオブジェクトは離すようにする
            if (pushedBtn_ != null)
                pushedBtn_.OnPointerExit();

            downedBtn_ = pushedBtn_ = null;
        }
    }
}