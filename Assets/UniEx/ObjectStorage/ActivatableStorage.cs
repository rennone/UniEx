using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Assertions;

// IActivatableを保存しておくためのクラス
// 前に使ったGameObjectを使いまわしたい場合などに使う
public class ActivatableStorage<T> where T : class, IActivatable
{
	public const int infinityLimit_ = -1;

	// オブジェクトを保持するリスト
	HashSet<T> Storage { get; set; }

	// 容量を超えた時に、破壊予定のオブジェクトが入るリスト
	HashSet<T> StorageForDestroy { get; set; }

	int StorageLimit { get; set; }
	bool NoLimit { get { return StorageLimit < 0; } }

	// ファクトリメソッド
	public readonly Func<T> factory_;

	// ファクトリメソッドを渡す
	/// <summary> ファクトリメソッドとストレージ容量を渡す. 負数だと∞の容量s </summary>
	public ActivatableStorage(Func<T> factory, int storageLimit = infinityLimit_)
	{
		factory_ = factory;
		Assert.IsNotNull(factory_, "factory method is null");

		StorageLimit = storageLimit;
		Storage = new HashSet<T>();
		StorageForDestroy = new HashSet<T>();
	}
	
	/// <summary> 新しいオブジェクトを取得 </summary>
	public virtual T Spawn()
	{
		// 非アクティブなオブジェクトを探す
		var ret = Storage.FirstOrDefault(s => s.GetActive() == false);

		// 無かったらファクトリメソッドで作る
		if(ret == null)
		{
			ret = factory_();

			// 容量を超えていない場合はStorageに保存
			if (NoLimit || Storage.Count < StorageLimit)
			{
				Storage.Add(ret);
			}
			// 満杯の場合はForDestroyに保存
			else {
				StorageForDestroy.Add(ret);
			}
		}

		// アクティブ状態にして返す
		ret.SetActive(true);
		return ret;
	}
	
	/// <summary> 全部非アクティブ or Destroyする</summary>
	public virtual void RecallAll()
	{
		foreach (var s in Storage)
			s.SetActive(false);

		// 容量を超えた分は破壊する
		foreach (var d in StorageForDestroy)
			d.Destroy();

		StorageForDestroy.Clear();
	}

	/// <summary> 個別に非アクティブ or Destroyする </summary>
	public virtual void Recall(T objectForDestroy)
	{
		objectForDestroy.SetActive(false);

		// Storageにないものは破壊する
		if( !Storage.Contains(objectForDestroy))
		{
			// 破壊予定のリストから削除する.(存在しなくても無視される)
			StorageForDestroy.Remove(objectForDestroy);

			// 破壊する
			objectForDestroy.Destroy();
		}
	}

	/// <summary>　現在アクティブなオブジェクトをすべて返す </summary>
	public virtual IEnumerable<T> ActiveObjects { get { return Storage.Where(a => a.GetActive()); } }
}

/// <summary> GameObject用のActivatableStorage. カスタムしなければこれを使えばいい. </summary>
public class ActivatableGameObjectStorage : ActivatableStorage<ActivatableGameObject>
{
	public ActivatableGameObjectStorage(GameObject prefab, Transform tr, bool worldPositionStay, int storageLimit = infinityLimit_)
		:base(ActivatableObject.GetFactory(prefab, tr, worldPositionStay), storageLimit)
	{}

	public ActivatableGameObjectStorage(Func<GameObject> factory, int storageLimit = infinityLimit_)
		:base( () => new ActivatableGameObject(factory()), storageLimit)
	{ }
}

/// <summary> Component用のActivatableStorage. カスタムしなければこれを使えばいい.  </summary>
public class ActivatableObjectStorage<T> : ActivatableStorage<ActivatableObject<T>> where T : Component
{
	public ActivatableObjectStorage(T prefab, Transform tr, bool worldPositionStay, int storageLimit = infinityLimit_)
		:base(ActivatableObject.GetFactory(prefab, tr, worldPositionStay), storageLimit)
	{ }

	public ActivatableObjectStorage(Func<T> factory, int storageLimit = infinityLimit_)
	: base(() => new ActivatableObject<T>(factory()), storageLimit)
	{ }
}

/// <summary>
/// UnityComponentのIActivatable実装
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActivatableObject<T> : IActivatable where T : Component
{
	T instance_;
	public T I { get { return instance_; } }

	public ActivatableObject(T component)
	{
		instance_ = component;
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
		GameObject.Destroy(I.gameObject);
	}
}

/// <summary>
/// Factoryの生成用クラス。GetFactoryを呼び出すときに明示的にジェネリック型を書かないで良いようにするためのクラス
/// </summary>
public static class ActivatableObject
{
	public static Func<ActivatableObject<T>> GetFactory<T>(T prefab, Transform tr, bool worldPositionStay) where T : Component
	{
		return () =>
		{
			var obj = GameObject.Instantiate<T>(prefab);
			obj.transform.SetParent(tr, worldPositionStay);
			return new ActivatableObject<T>(obj);
		};
	}

	public static Func<ActivatableObject<T>> GetFactory<T>(GameObject prefab, Transform tr, bool worldPositionStay) where T : Component
	{
		return () =>
		{
			var obj = GameObject.Instantiate(prefab, tr, worldPositionStay) as GameObject;
			return new ActivatableObject<T>(obj.GetComponent<T>());
		};
	}

	public static Func<ActivatableGameObject> GetFactory(GameObject prefab, Transform tr, bool worldPositionStay)
	{
		return () =>
		{
			var obj = GameObject.Instantiate(prefab, tr, worldPositionStay) as GameObject;
			return new ActivatableGameObject(obj);
		};
	}
}

/// <summary>
/// GameObjectのIActivatable実装
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActivatableGameObject : IActivatable
{
	GameObject instance_;
	public GameObject I { get { return instance_; } }

	public ActivatableGameObject(GameObject instance)
	{
		instance_ = instance;
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
		GameObject.Destroy(I.gameObject);
	}
}

/// <summary> MonoBehaviourのIActivatable実装 </summary>
public class ActivatableBehaviour : MonoBehaviour, IActivatable
{
	public virtual void Destroy()
	{
		GameObject.Destroy(gameObject);
	}

	public virtual bool GetActive()
	{
		return gameObject.activeSelf;
	}

	public virtual void SetActive(bool enable)
	{
		gameObject.SetActive(enable);
	}
}
