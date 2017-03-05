using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

// プレハブのキャッシュデータ
[System.Serializable]
public class ObjectCache
{
	//キャッシュサイズ
    [SerializeField]
    int cacheSize_;
    public int CacheSize { get { return cacheSize_; } }

	// キャッシュするプレハブ
    [SerializeField]
    GameObject prefab_;
    public GameObject Prefab { get { return prefab_; } }

	// キャッシュリスト
	HashSet<GameObject> cachedObjects_ = new HashSet<GameObject>();
	HashSet<GameObject> CachedObjects { get
		{
			if (cachedObjects_ == null)
				cachedObjects_ = new HashSet<GameObject>();
			return cachedObjects_;
		} }

	/// <summary>
	/// キャッシュ中にpredictで指定したオブジェクトがいるかチェック
	/// </summary>
	public bool AnyInCache( Func<GameObject, bool> predict)
	{
		return CachedObjects.Any(predict);
	}

	// 使われていないプレハブを取得する
	// 無かった場合はInstantiateしてとってくる.
    public GameObject GetNextObjectInCache()
    {
		// null(外部により削除されたオブジェクトを消す)
		CachedObjects.RemoveWhere(x => x == null);

        foreach(var obj in CachedObjects)
        {
            if (!obj.activeSelf)
                return obj;
        }

        var ret = GameObject.Instantiate(Prefab);

		if (CachedObjects.Count < cacheSize_)
		{
			CachedObjects.Add(ret);
		}

        return ret;
    }

	public bool RecallToCache(GameObject objectToDestroy)
	{
		if(CachedObjects.Contains(objectToDestroy))
		{
			objectToDestroy.SetActive(false);
			return true;
		}

		// キャッシュに入りきらない場合は破壊する
		GameObject.Destroy(objectToDestroy);
		return false;
	}

	/// <summary>
	/// キャッシュを全削除(Destroy)
	/// </summary>
	public void Clear()
	{
		CachedObjects.RemoveWhere(x => x == null);

		foreach (var obj in CachedObjects)
			GameObject.Destroy(obj);

		CachedObjects.Clear();
	}

	public static ObjectCache Create(GameObject prefab, int cacheSize)
	{
		return new ObjectCache() { cacheSize_ = cacheSize, prefab_ = prefab };
	}
}
