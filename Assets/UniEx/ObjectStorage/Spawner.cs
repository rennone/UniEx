using UnityEngine;
using System;

namespace UniEx
{
    [Serializable]
    public abstract class SpawnerBase<T> where T : UnityEngine.Object
    {
        [SerializeField]
        int cacheSize_;

        public int CacheSize
        {
            get { return cacheSize_; }
        }

        // キャッシュするプレハブ
        [SerializeField]
        T prefab_;

        public T Prefab
        {
            get { return prefab_; }
        }

        protected SpawnerBase(T prefab, int cacheSize)
        {
            cacheSize_ = cacheSize;
            prefab_ = prefab;
        }

        public abstract T Spawn();
        public abstract bool Recall(T objectToDestroy);
        public abstract void Clear();
    }


    [Serializable]
    public class Spawner : SpawnerBase<GameObject>
    {
        /// <summary>
        /// 内部的に持つGameObjectStorage
        /// </summary>
        private GameObjectStorage Storage { get; set; }

        public Spawner(GameObject prefab, int cacheSize) : base(prefab, cacheSize) { }

        /// <summary>
        /// 新しくインスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public override GameObject Spawn()
        {
            if (Storage == null)
                Storage = new GameObjectStorage(Prefab, null, true, CacheSize);

            return Storage.Spawn();
        }

        /// <summary>
        /// キャッシュに戻す
        /// </summary>
        /// <param name="objectToDestroy"></param>
        /// <returns></returns>
        public override bool Recall(GameObject objectToDestroy)
        {
            if (Storage == null)
                Storage = new GameObjectStorage(Prefab, null, true, CacheSize);

            return Storage.Recall(objectToDestroy);
        }

        /// <summary>
        /// キャッシュを全削除(Destroy)
        /// </summary>
        public override void Clear()
        {
            if (Storage != null)
                Storage.DestroyAll();
        }
    }

    [Serializable]
    public class Spawner<T> : SpawnerBase<T> where T : Component
    {
        /// <summary>
        /// 内部的に持つGameObjectStorage
        /// </summary>
        private ComponentStorage<T> Storage { get; set; }

        public Spawner(T prefab, int cacheSize) : base(prefab, cacheSize) { }

        /// <summary>
        /// 新しくインスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public override T Spawn()
        {
            if (Storage == null)
                Storage = new ComponentStorage<T>(Prefab, null, true, CacheSize);

            return Storage.Spawn();
        }

        /// <summary>
        /// キャッシュに戻す
        /// </summary>
        /// <param name="objectToDestroy"></param>
        /// <returns></returns>
        public override bool Recall(T objectToDestroy)
        {
            if (Storage == null)
                Storage = new ComponentStorage<T>(Prefab, null, true, CacheSize);

            return Storage.Recall(objectToDestroy);
        }

        /// <summary>
        /// キャッシュを全削除(Destroy)
        /// </summary>
        public override void Clear()
        {
            if (Storage != null)
                Storage.DestroyAll();
        }
    }
}