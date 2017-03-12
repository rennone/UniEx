using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace UniEx
{
    // ActivatableStorageで使うために実装するインターフェース
    // アクティブのオンオフと取得
    public interface IActivatable
    {
        /// <summary> アクティブの切り替え </summary>
        void SetActive(bool enable);

        /// <summary> 現在のアクティブを取得 </summary>
        bool GetActive();

        /// <summary> オブジェクト自体を破壊する </summary>
        void Destroy();
    }

    /// <summary>
    /// UnityComponentのIActivatable実装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActivatableComponent<T> : IActivatable, IEquatable<ActivatableComponent<T>> where T : Component
    {
       
        public T I { get; }

        public ActivatableComponent(T component)
        {
            Assert.IsNotNull(component, "instance is null");
            I = component;
        }

        public virtual bool GetActive()
        {
            return I.gameObject.activeSelf;
        }

        public virtual void SetActive(bool enable)
        {
            I.gameObject.SetActive(enable);
        }

        public virtual void Destroy()
        {
            Object.Destroy(I.gameObject);
        }

        /// <summary>
        /// 同じオブジェクトを参照するActivatableComponentが2つストレージに入ると困るので, 参照先が同じなら同等に扱う
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ActivatableComponent<T> other)
        {
            return other != null && other.I == I;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ActivatableComponent<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(I);
        }


        public static bool operator==(ActivatableComponent<T> lhs, ActivatableComponent<T> rhs)
        {
            // 両方null もしくは同じインスタンスチェック
            if (object.ReferenceEquals(lhs, rhs))
                return true;

            // どっちか一方がnullの場合はfalse. そうでなければEqual関数で比べる
            return (object)lhs != null && lhs.Equals(rhs);
        }

        public static bool operator!=(ActivatableComponent<T> lhs, ActivatableComponent<T> rhs)
        {
            return !(lhs == rhs);
        }
    }

    /// <summary>
    /// GameObjectのIActivatable実装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActivatableGameObject : IActivatable, IEquatable<ActivatableGameObject>
    {
       

        public GameObject I { get; }

        public ActivatableGameObject(GameObject instance)
        {
            Assert.IsNotNull(instance, "instance is null");
            I = instance;
        }

        public virtual bool GetActive()
        {
            return I.activeSelf;
        }

        public virtual void SetActive(bool enable)
        {
            I.SetActive(enable);
        }

        public virtual void Destroy()
        {
            Object.Destroy(I.gameObject);
        }

        /// <summary>
        /// 同じオブジェクトを参照するActivatableComponentが2つストレージに入ると困るので, 参照先が同じなら同等に扱う
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ActivatableGameObject other)
        {
            return other != null && I == other.I;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ActivatableGameObject)obj);
        }

        public override int GetHashCode()
        {
            return (I != null ? I.GetHashCode() : 0);
        }

        public static bool operator ==(ActivatableGameObject lhs, ActivatableGameObject rhs)
        {
            // 両方null もしくは同じインスタンスチェック
            if (object.ReferenceEquals(lhs, rhs))
                return true;
            
            // どっちか一方がnullの場合はfalse. そうでなければEqual関数で比べる
            return (object)lhs != null && lhs.Equals(rhs);
        }

        public static bool operator !=(ActivatableGameObject lhs, ActivatableGameObject rhs)
        {
            return !(lhs == rhs);
        }
    }
}