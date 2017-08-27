using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UniEx
{
    public static class TransformExtensions
    {
        /// <summary>
        /// 拡張機能 : 子を全削除する
        /// </summary>
        public static void DestroyChildren(this Transform tr)
        {
            while (tr.childCount != 0)
            {
                var child = tr.GetChild(0);
                child.SetParent(null);
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// 拡張機能 : ポジションをVector2で返す
        /// </summary>
        public static Vector2 GetPos2D(this Transform tr)
        {
            return tr.position;
        }

        /// <summary>
        /// 拡張機能 : ポジションをVector2で設定する
        /// </summary>
        public static void SetPos2D(this Transform tr, Vector2 pos)
        {
            tr.position = new Vector3(pos.x, pos.y, tr.position.z);
        }

        /// <summary>
        /// position.zを設定する
        /// </summary>
        public static void SetPosZ(this Transform tr, float z)
        {
            tr.position = new Vector3(tr.position.x, tr.position.y, z);
        }

        /// <summary>
        /// position.xを設定する
        /// </summary>
        public static void SetPosX(this Transform tr, float x)
        {
            tr.position = new Vector3(x, tr.position.y, tr.position.z);
        }

        /// <summary>
        /// position.yを設定する
        /// </summary>
        public static void SetPosY(this Transform tr, float y)
        {
            tr.position = new Vector3(tr.position.x, y, tr.position.z);
        }

        /// <summary>
        /// 拡張機能 : ローカルスケールをVector2Dで設定する
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="localScale"></param>
        public static void SetLocalScale2D(this Transform tr, Vector2 localScale)
        {
            tr.transform.localScale = new Vector3(localScale.x, localScale.y, tr.localScale.z);
        }

        public static Vector2 GetLocalScale2D(this Transform tr)
        {
            return tr.localScale;
        }

        /// <summary>
        /// localScale.xを返す
        /// </summary>
        public static float GetLocalScaleX(this Transform tr)
        {
            return tr.localScale.x;
        }

        /// <summary>
        /// localScale.yを返す
        /// </summary>
        public static float GetLocalScaleY(this Transform tr)
        {
            return tr.localScale.y;
        }

        /// <summary>
        /// localScale.zを返す
        /// </summary>
        public static float GetLocalScaleZ(this Transform tr)
        {
            return tr.localScale.z;
        }

        /// <summary>
        /// localPosition.xを設定する
        /// </summary>
        public static void SetLocalScaleX(this Transform tr, float x)
        {
            tr.localScale = new Vector3(x, tr.localScale.y, tr.localScale.z);
        }

        /// <summary>
        /// localPosition.yを設定する
        /// </summary>
        public static void SetLocalScaleY(this Transform tr, float y)
        {
            tr.localScale = new Vector3(tr.localScale.x, y, tr.localScale.z);
        }

        /// <summary>
        /// localPosition.zを設定する
        /// </summary>
        public static void SetLocalScaleZ(this Transform tr, float z)
        {
            tr.localScale = new Vector3(tr.localScale.x, tr.localScale.y, z);
        }

        /// <summary>
        /// 拡張機能 : ローカルポジションをVector2で返す
        /// </summary>
        public static Vector2 GetLocalPos2D(this Transform tr)
        {
            return tr.localPosition;
        }

        /// <summary>
        /// localPosition.zを返す
        /// </summary>
        public static float GetLocalPosZ(this Transform tr)
        {
            return tr.localPosition.z;
        }

        /// <summary>
        /// localPosition.xを返す
        /// </summary>
        public static float GetLocalPosX(this Transform tr)
        {
            return tr.localPosition.x;
        }

        /// <summary>
        /// localPosition.yを返す
        /// </summary>
        public static float GetLocalPosY(this Transform tr)
        {
            return tr.localPosition.y;
        }

        /// <summary>
        /// 拡張機能 : ローカルポジションをVector2で設定する
        /// </summary>
        public static void SetLocalPos2D(this Transform tr, Vector2 pos)
        {
            tr.transform.localPosition = new Vector3(pos.x, pos.y, tr.localPosition.z);
        }

        /// <summary>
        /// localPosition.zを設定する
        /// </summary>
        public static void SetLocalPosZ(this Transform tr, float z)
        {
            tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, z);
        }

        /// <summary>
        /// localPosition.xを設定する
        /// </summary>
        public static void SetLocalPosX(this Transform tr, float x)
        {
            tr.localPosition = new Vector3(x, tr.localPosition.y, tr.localPosition.z);
        }

        /// <summary>
        /// localPosition.yを設定する
        /// </summary>
        public static void SetLocalPosY(this Transform tr, float y)
        {
            tr.localPosition = new Vector3(tr.localPosition.x, y, tr.localPosition.z);
        }

        /// <summary>
        /// localEulerAngles.zを返す
        /// </summary>
        public static float GetLocalEulerAngleZ(this Transform tr, float z)
        {
            return tr.localEulerAngles.z;
        }

        /// <summary>
        /// localEulerAngles.zを設定する
        /// </summary>
        public static void SetLocalEulerAndleZ(this Transform tr, float z)
        {
            tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y, z);
        }

        /// <summary>
        /// localEulerAngles.xを返す
        /// </summary>
        public static float GetLocalEulerAngleX(this Transform tr, float x)
        {
            return tr.localEulerAngles.x;
        }

        /// <summary>
        /// localEulerAngles.xを設定する
        /// </summary>
        public static void SetLocalEulerAndleX(this Transform tr, float x)
        {
            tr.localEulerAngles = new Vector3(x, tr.localEulerAngles.y, tr.localEulerAngles.z);
        }

        /// <summary>
        /// localEulerAngles.yを返す
        /// </summary>
        public static float GetLocalEulerAngleY(this Transform tr, float y)
        {
            return tr.localEulerAngles.y;
        }

        /// <summary>
        /// localEulerAngles.yを設定する
        /// </summary>
        public static void SetLocalEulerAndleY(this Transform tr, float y)
        {
            tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, y, tr.localEulerAngles.z);
        }

        /// <summary>
        /// FindChildとGetComponentを同時に行う. 
        /// FindChildが失敗した場合もnullを返す
        /// </summary>
        public static T GetChildComponent<T>(this Transform tr, string path) where T : Component
        {
            var child = tr.Find(path);

            if (child == null)
            {
                Debug.LogWarning("can not find child " + path + " of " + tr.GetFullpathName());
            }

            return child == null ? null : child.GetComponent<T>();
        }

        /// <summary>
        /// FindChildとGetOrAddComponentを同時に行う. 
        /// FindChildが失敗した場合にのみnullを返す
        /// </summary>
        public static T GetOrAddChildComponent<T>(this Transform tr, string path) where T : Component
        {
            var child = tr.Find(path);
            if (child == null)
            {
                Debug.LogWarning("can not find child " + path + " of " + tr.GetFullpathName());
            }

            return child == null ? null : child.GetOrAddComponent<T>();
        }

        /// <summary>
        /// GetComponentに自身(Transform)のnullチェックを加えたもの
        /// </summary>
        public static T GetComponentOrDefault<T>(this Transform tr) where T : Component
        {
            return tr == null ? null : tr.GetComponent<T>();
        }

        /// <summary>
        /// GetComponentInChildrenに自身(Transform)のnullチェックを加えたもの
        /// </summary>
        public static T GetComponentInChildrenOrDefault<T>(this Transform tr) where T : Component
        {
            return tr == null ? null : tr.GetComponentInChildren<T>();
        }

        /// <summary>
        /// GetComponentInParentに自身(Transform)のnullチェックを加えたもの
        /// </summary>
        public static T GetComponentInParentOrDefault<T>(this Transform tr) where T : Component
        {
            return tr == null ? null : tr.GetComponentInParent<T>();
        }

        /// <summary>
        /// tr.gameObject.GetOrAddComponent<T>()と同値
        /// </summary>
        public static T GetOrAddComponent<T>(this Transform tr) where T : Component
        {
            return tr.gameObject.GetOrAddComponent<T>();
        }

        /// <summary>
        /// tr.gameObject.GetOrAddComponentOrDefault<T>()にtrのnullチェック追加
        /// </summary>
        public static T GetOrAddComponentOrDefault<T>(this Transform tr) where T : Component
        {
            return tr == null ? null : tr.gameObject.GetOrAddComponentOrDefault<T>();
        }

        /// <summary>
        /// 現在のオブジェクトのルートオブジェクトからのパスを取得. 基本はUnityEditor用
        /// </summary>
        public static string GetFullpathName(this Transform tr)
        {
            if (tr == null)
                return string.Empty;

            if (tr.parent == null)
                return tr.name;

            return tr.parent.GetFullpathName() + "/" + tr.name;
        }

        /// <summary> アクティブなchildの個数を返す </summary>
        public static int GetActiveChildCount(this Transform self)
        {
            int count = 0;
            foreach (Transform child in self)
                if (child.gameObject.activeSelf)
                    count++;

            return count;
        }

        /// <summary> アクティブなchildが存在するか. </summary>
        public static bool HasActiveChild(this Transform self)
        {
            foreach (Transform child in self)
                if (child.gameObject.activeSelf)
                    return true;

            return false;
        }

        /// <summary> GetComponentsInChildrenで自分自身を含まずに探索する。 </summary>
        public static T[] GetComponentsInChildrenWithoutSelf<T>(this Transform self) where T : Component
        {
            if (self == null)
                return new T[] {};

            return self.GetComponentsInChildren<T>().Where(c => self.gameObject != c.gameObject).ToArray();
        }

        /// <summary> 自身の子供をすべて返す. activeがfalseの物を含む </summary>
        public static IEnumerable<Transform> GetChildren(this Transform self)
        {
            foreach (Transform child in self)
                yield return child;
        }
    }
}