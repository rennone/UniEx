using UnityEngine;
using System;
using CsEx;
namespace UniEx
{
    public abstract class CachedObject<T> where T : UnityEngine.Object
    {
        public string Path { get; private set; }

        T cache_;

        protected CachedObject(string path = "")
        {
            Path = path;
        }

        public T I(Transform root)
        {
            if (cache_ != null)
                return cache_;

            cache_ = GetObject(root);

            if (cache_ == null)
            {
                Debug.LogWarning($"transform {Path} not found");
            }

            return cache_;
        }

        public void Clear()
        {
            cache_ = null;
        }

        protected abstract T GetObject(Transform root);
    }

    /// <summary> TransformのFindChildをキャッシュしたクラス </summary> 
    public class CachedTransform : CachedObject<Transform>
    {
        public CachedTransform(string path = "") : base(path)
        {
        }

        protected override Transform GetObject(Transform root)
        {
            return root.FindChild(Path);
        }
    }

    /// <summary> GameObjectのキャッシュクラス </summary> 
    public class CachedGameObject : CachedObject<GameObject>
    {
        public CachedGameObject(string path = "") : base(path)
        {
        }

        protected override GameObject GetObject(Transform root)
        {
            var tr = root.FindChild(Path);
            return tr == null ? null : tr.gameObject;
        }
    }

    [System.Serializable]
    public class CachedComponent<T> where T : Component
    {
        [SerializeField] T component_;

        CachedTransform finder_;


        public CachedComponent(string path = "")
        {
            finder_ = new CachedTransform(path);
        }

        T Getter(Transform root, Func<Transform, T> func)
        {
            // すでにキャッシュに入っていたらそれを返す
            if (component_ != null)
                return component_;

            // 指定したPathのトランスフォームを取得
            var tr = finder_.I(root);

            // そっから引数の関数でComponentを探す
            component_ = tr == null ? null : func(tr);

            if (component_ == null)
            {
                Debug.Log(root.name + "/{0}".Formats(finder_.Path) + " don't have component " + typeof(T).Name);
            }

            return component_;
        }

        /// <summary>
        /// rootからGetComponentで探す.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public T GetComponent(Transform root)
        {
            return Getter(root, tr => tr.GetComponent<T>());
        }

        /// <summary>
        /// rootからGetOrAddComponentで探す
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public T GetOrAddComponent(Transform root)
        {
            return Getter(root, tr => tr.GetOrAddComponent<T>());
        }

        /// <summary>
        /// rootからGetComponentInChildrenで探す
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public T GetComponentInChildren(Transform root)
        {
            return Getter(root, tr => tr.GetComponentInChildren<T>());
        }

        /// <summary>
        /// rootからGetComponentInParentで探す
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public T GetComponentInParent(Transform root)
        {
            return Getter(root, tr => tr.GetComponentInParent<T>());
        }

        public void Clear()
        {
            component_ = null;
            finder_.Clear();
        }
    }

    public class CachedStateMachineBehaviour<T> where T : StateMachineBehaviour
    {
        T behaviour_;

        public T GetBehaviour(Animator animator)
        {
            if (behaviour_ == null)
                behaviour_ = animator.GetBehaviour<T>();

            if (behaviour_ == null)
            {
                Debug.Log(animator.name + " dont have behabiour " + typeof(T).Name);
            }

            return behaviour_;
        }
    }
}