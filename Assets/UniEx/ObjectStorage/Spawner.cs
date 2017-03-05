using UnityEngine;
using System;

namespace UniEx
{

    [Serializable]
    public class Spawner
    {
        //キャッシュサイズ
        [SerializeField] int cacheSize_;

        public int CacheSize
        {
            get { return cacheSize_; }
        }

        // キャッシュするプレハブ
        [SerializeField] GameObject prefab_;

        public GameObject Prefab
        {
            get { return prefab_; }
        }

        /// <summary>
        /// 内部的に持つGameObjectStorage
        /// </summary>
        private GameObjectStorage Storage { get; set; }

        /// <summary>
        /// 新しくインスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public GameObject Spawn()
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
        public bool Recall(GameObject objectToDestroy)
        {
            if (Storage == null)
                Storage = new GameObjectStorage(Prefab, null, true, CacheSize);

            return Storage.Recall(objectToDestroy);
        }

        /// <summary>
        /// キャッシュを全削除(Destroy)
        /// </summary>
        public void Clear()
        {
            if (Storage != null)
                Storage.DestroyAll();
        }

        public static Spawner Create(GameObject prefab, int cacheSize)
        {
            return new Spawner() {cacheSize_ = cacheSize, prefab_ = prefab};
        }
    }
}