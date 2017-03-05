using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Spawner
{
    [SerializeField]
    ObjectCache cache_;

	public int CacheSize { get { return cache_.CacheSize; } }

	public GameObject Prefab { get { return cache_.Prefab; } }
	
    // 生成
    public GameObject Spawn()
    {
        if (cache_ == null)
        {
            Debug.Log("ObjectCache is null");
            return null;
        }

		// Find the next object in the cache
        GameObject obj = cache_.GetNextObjectInCache();
		obj.SetActive(true);
		
		return obj;
    }

	/// <summary> Spawnの親設定を含む版 </summary>
	public GameObject Spawn(Transform parent, bool worldPositionStay)
	{
		var ret = Spawn();
		if (ret != null)
			ret.transform.SetParent(parent, worldPositionStay);

		return ret;
	}

	/// <summary> Spawn + GetComponent </summary>
	public T Spawn<T>() where T : Component
	{
		var ret = Spawn();
		return ret != null ? ret.GetComponent<T>() : null;
	}

	/// <summary> Spawn(Transform,bool) + GetComponent </summary>
	public T Spawn<T>(Transform parent, bool worldPositionStay) where T : Component
	{
		var ret = Spawn(parent, worldPositionStay);
		return ret != null ? ret.GetComponent<T>() : null;
	}

    // 回収
    public void Recall(GameObject objectToDestroy)
    {
		cache_.RecallToCache(objectToDestroy);
    }

	/// <summary>
	/// キャッシュを全削除(Destroy)
	/// </summary>
	public void Clear()
	{
		cache_.Clear();
	}

	/// <summary>
	/// キャッシュ中にpredictで指定したオブジェクトがいるかチェック
	/// </summary>
	public bool AnyInCache(Func<GameObject, bool> predict)
	{
		return cache_.AnyInCache(predict);
	}

	public static Spawner Create(GameObject prefab, int cacheSize)
	{
		return new Spawner() { cache_ = ObjectCache.Create(prefab, cacheSize) };
	}
}