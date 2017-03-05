using UnityEngine;
using System.Collections;

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
