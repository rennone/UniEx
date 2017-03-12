using UnityEngine;
using System.Collections;

namespace UniEx
{
    // UnityEngine.GameObjectの拡張メソッド
    public static class GameObjectEx
    {
        /// <summary>
        /// GetComponentに失敗した場合に, AddComponentを行う拡張関数
        /// </summary>
        public static TComponent GetOrAddComponent<TComponent>(this GameObject instance) where TComponent : Component
        {
            var ret = instance.GetComponent<TComponent>();
            return ret != null ? ret : instance.AddComponent<TComponent>();
        }

        /// <summary>
        /// GetOrAddComponentに呼び出したgameObjectのnullチェックを追加した拡張関数
        /// </summary>
        public static T GetOrAddComponentOrDefault<T>(this GameObject self) where T : Component
        {
            return self == null ? null : self.GetOrAddComponent<T>();
        }

        /// <summary>
        /// 現在のオブジェクトのルートオブジェクトからのパスを取得. 基本はUnityEditor用
        /// </summary>
        public static string GetFullpathName(this GameObject obj)
        {
            return obj == null ? string.Empty : obj.transform.GetFullpathName();
        }
    }
}