using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// アクティブな
// ストレージに保持しないタイプのSpawner
public class PrefabStorage<T> where T : Component
{
	// インスタンス化するプレハブ
	T prefab_;

	// activeSelfがfalseの物を保存する場所
	HashSet<T> storage_ = new HashSet<T>();

	public PrefabStorage(T prefab)
	{
		prefab_ = prefab;
	}

	// 配布
	public T Spawn()
	{
		T ret = null;

		foreach (var p in storage_)
		{
			if (p.gameObject.activeSelf)
				continue;
			ret = p;
			ret.gameObject.SetActive(true);
			break;
		}

		// なるべく検索に時間かけない様に(てか1回で見つかるように)
		// 配布したものはストレージから削除する
		if(ret != null)
			storage_.Remove(ret);
		else
			ret = Object.Instantiate<T>(prefab_);

		return ret;
	}

	// 回収
	public void Recall(ref T obj)
	{
		if (obj == null)
			return;

		// 参照先
		T o = obj;
		o.gameObject.SetActive(false);
		storage_.Add(o);

		// 呼び出し元が保持したままだと困るのでnullを入れる
		obj = null;

	}
}
