using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace UniEx
{
    // IActivatableを保存しておくためのクラス
    // 前に使ったGameObjectを使いまわしたい場合などに使う
    public class ActivatableStorage<T> where T : class, IActivatable
    {
        public const int InfinityLimit = -1;

        // オブジェクトを保持するリスト
        private HashSet<T> Storage { get; }

        // 容量を超えた時に、破壊予定のオブジェクトが入るリスト
        private HashSet<T> StorageForDestroy { get; set; }

        public int StorageLimit { get; }

        public bool NoLimit { get { return StorageLimit < 0; } }

        // ファクトリメソッド
        private Func<T> Factory { get; }


        // ファクトリメソッドを渡す
        /// <summary> ファクトリメソッドとストレージ容量を渡す. 負数だと∞の容量s </summary>
        public ActivatableStorage(Func<T> factory, int storageLimit = InfinityLimit)
        {
            Factory = factory;
            Assert.IsNotNull(Factory, "Factory method is null");

            StorageLimit = storageLimit;
            Storage = new HashSet<T>();
            StorageForDestroy = new HashSet<T>();
        }

        /// <summary> 新しいインスタンスを取得 </summary>
        public virtual T NextObject()
        {
            // 外部により破壊されたものは削除しておく
            Storage.RemoveWhere(x => x == null);

            // 非アクティブなオブジェクトを探す
            var ret = Storage.FirstOrDefault(s => s.GetActive() == false);

            // 無かったらファクトリメソッドで作る
            if (ret == null)
            {
                ret = Factory();

                Assert.IsNotNull(ret, "Factory method return null");

                // 容量を超えていない場合はStorageに保存
                if (NoLimit || Storage.Count < StorageLimit)
                {
                    Storage.Add(ret);
                }
                // 満杯の場合はForDestroyに保存
                else
                {
                    StorageForDestroy.Add(ret);
                }
            }

            // アクティブ状態にして返す
            ret.SetActive(true);
            return ret;
        }

        /// <summary> 全部非アクティブ or Destroyする</summary>
        public virtual void DisableAll()
        {
            // 外部により破壊されたものは削除しておく
            Storage.RemoveWhere(x => x == null);

            foreach (var s in Storage)
                s.SetActive(false);

            // 容量を超えた分は破壊する
            foreach (var d in StorageForDestroy.Where(x => x != null))
                d.Destroy();

            StorageForDestroy.Clear();
        }

        /// <summary> 個別に非アクティブ or Destroyする. 破壊された場合falseを返す. </summary>
        public virtual bool RecallObject(T objectForDestroy)
        {
            if (objectForDestroy == null)
                return true;

            // Storageにあるものはアクティブだけfalseにする
            if (Storage.Contains(objectForDestroy))
            {
                objectForDestroy.SetActive(false);
                return true;
            }

            // Storageにないものは破壊する
            // 破壊予定のリストから削除する.(存在しなくても無視される)
            StorageForDestroy.Remove(objectForDestroy);

            // 破壊する
            objectForDestroy.Destroy();

            return false;
        }

        /// <summary>
        /// Storageにある文も含めてすべてDestroyする.
        /// </summary>
        public virtual void DestroyAll()
        {
            foreach (var child in Storage.Where(x => x != null))
            {
                child.Destroy();
            }

            foreach (var child in StorageForDestroy.Where(x => x != null))
            {
                child.Destroy();
            }

            Storage.Clear();
            StorageForDestroy.Clear();
        }

        /// <summary>　現在アクティブなオブジェクトをすべて返す </summary>
        public virtual IEnumerable<T> ActiveObjects
        {
            get { return Storage.Where(a => a.GetActive()); }
        }
    }

    /// <summary> GameObject用のActivatableStorage. カスタムしなければこれを使えばいい. </summary>
    public class GameObjectStorage : ActivatableStorage<ActivatableGameObject>
    {
        public GameObjectStorage(GameObject prefab, Transform tr, bool worldPositionStay = true,
            int storageLimit = InfinityLimit)
            : base(ActivatableStorage.GetFactory(prefab, tr, worldPositionStay), storageLimit)
        {
        }

        public GameObjectStorage(Func<GameObject> factory, int storageLimit = InfinityLimit)
            : base(() => new ActivatableGameObject(factory()), storageLimit)
        {
        }

        /// <summary>
        /// キャッシュから新しいインスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public GameObject Spawn()
        {
            return NextObject().I;
        }

        /// <summary>
        /// キャッシュに戻す. 戻されず破壊された場合falseを返す
        /// </summary>
        /// <param name="objectForDestroy"></param>
        /// <returns></returns>
        public bool Recall(GameObject objectForDestroy)
        {
            return RecallObject(new ActivatableGameObject(objectForDestroy));
        }
    }

    /// <summary> Component用のActivatableStorage. カスタムしなければこれを使えばいい.  </summary>
    public class ComponentStorage<T> : ActivatableStorage<ActivatableComponent<T>> where T : Component
    {
        public ComponentStorage(T prefab, Transform tr, bool worldPositionStay = true,
            int storageLimit = InfinityLimit)
            : base(ActivatableStorage.GetFactory(prefab, tr, worldPositionStay), storageLimit)
        {
        }

        public ComponentStorage(Func<T> factory, int storageLimit = InfinityLimit)
            : base(() => new ActivatableComponent<T>(factory()), storageLimit)
        {
        }

        /// <summary>
        /// キャッシュから新しいインスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public T Spawn()
        {
            return NextObject().I;
        }

        /// <summary>
        /// キャッシュに戻す. 戻されず破壊された場合falseを返す
        /// </summary>
        /// <param name="objectForDestroy"></param>
        /// <returns></returns>
        public bool Recall(T objectForDestroy)
        {
            return RecallObject(new ActivatableComponent<T>(objectForDestroy));
        }
    }

    /// <summary>
    /// Factoryの生成用クラス。GetFactoryを呼び出すときに明示的にジェネリック型を書かないで良いようにするためのクラス
    /// </summary>
    public static class ActivatableStorage
    {
        public static Func<ActivatableComponent<T>> GetFactory<T>(T prefab, Transform tr, bool worldPositionStay)
            where T : Component
        {
            return () => new ActivatableComponent<T>(UnityEngine.Object.Instantiate(prefab, tr, worldPositionStay));
        }

        public static Func<ActivatableComponent<T>> GetFactory<T>(GameObject prefab, Transform tr, bool worldPositionStay)
            where T : Component
        {
            return () => new ActivatableComponent<T>(UnityEngine.Object.Instantiate(prefab, tr, worldPositionStay).GetComponent<T>());
        }

        public static Func<ActivatableGameObject> GetFactory(GameObject prefab, Transform tr, bool worldPositionStay)
        {
            return () => new ActivatableGameObject(UnityEngine.Object.Instantiate(prefab, tr, worldPositionStay));
        }
    }

   
}