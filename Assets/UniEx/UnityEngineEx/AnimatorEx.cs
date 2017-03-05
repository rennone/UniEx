using UnityEngine;
using System.Collections;

public static class AnimatorExtensions
{
	public static RuntimeAnimatorController LoadRuntimeAnimator(string path)
	{
		var animatorController = ResourceLoader.Load<RuntimeAnimatorController>(path);

		// AnimationClip書換用のoverrideAnimatorControllerの作成
		AnimatorOverrideController overrideAnimatorController = new AnimatorOverrideController();
		overrideAnimatorController.runtimeAnimatorController = animatorController;

		return overrideAnimatorController;
	}

	public static bool GetTrigger(this Animator self,  int id)
	{
		return self.GetBool(id);
	}

	public static bool GetTrigger(this Animator self, string id)
	{
		return self.GetBool(id);
	}

	/// <summary>
	/// アニメーション再生後指定ステートまで更新待機する
	/// </summary>
	/// <param name="self"></param>
	/// <param name="trigger"></param>
	/// <param name="state">指定しない場合トリガ名と同一のステート名に設定する</param>
	/// <returns></returns>
	public static IEnumerator PlayUpdate(this Animator self, string trigger, string state = null)
	{
		self.SetTrigger(trigger);
		if(state == null)
			state = trigger;
		yield return null;
		yield return new WaitWhile(() => self.IsState(state));
	}

	/// <summary>
	/// GetCurrentAnimatorStateInfo(0).IsName(state)と同値
	/// </summary>
	public static bool IsState(this Animator self, string state, int layerNo = 0)
	{
		return self.GetCurrentAnimatorStateInfo(layerNo).IsName(state);
	}

	/// <summary>
	/// GetCurrentAnimatorStateInfo(0).fulllPathhash == fullpathHashと同値
	/// </summary>
	public static bool IsState(this Animator self, int fullpathHash, int layerNo = 0)
	{
		return self.GetCurrentAnimatorStateInfo(layerNo).fullPathHash == fullpathHash;
	}

	/// <summary>
	/// 現在再生中のアニメーションが終わるのを待つ.
	/// ステートのパスと正規化時間で判定
	/// </summary>
	public static IEnumerator WaitForCurrentAnimation(this Animator self, int layerNo = 0)
	{
		var lastStateHash = self.GetCurrentAnimatorStateInfo(layerNo).fullPathHash;
		Debug.Log(lastStateHash);
		// 同じステート && 正規化再生時間が1未満
		yield return new WaitWhile(() => lastStateHash == self.GetCurrentAnimatorStateInfo(layerNo).fullPathHash && self.GetCurrentAnimatorStateInfo(layerNo).normalizedTime < 1);
	}

	/// <summary>
	/// WaitToTransitTo と WaitForCurrentAnimationの組み合わせ.
	/// stateに遷移するまで待つ → stateのアニメーションが終了するまで待つ(shortNameHashで比較)
	/// </summary>
	public static IEnumerator WaitForTransitAndEnd(this Animator self, string state, int layerNo = 0)
	{
		var hash = Animator.StringToHash(state);

		// stateに遷移するのを待つ
		yield return new WaitUntil( () => self.GetCurrentAnimatorStateInfo(layerNo).shortNameHash == hash);
		
		// stateのアニメーションが終了するのを待つ
		yield return new WaitWhile(() => hash == self.GetCurrentAnimatorStateInfo(layerNo).shortNameHash && self.GetCurrentAnimatorStateInfo(layerNo).normalizedTime < 1);
	}

	/// <summary>
	/// WaitUntil( () => self.IsState(state))と同値(ただし、高速化の為内部でハッシュ変換を行っている
	/// </summary>
	public static IEnumerator WaitToTransitTo(this Animator self, string state, int layerNo = 0)
	{		
		yield return self.WaitToTransitTo(Animator.StringToHash(state), layerNo);
	}

	/// <summary>
	/// WaitUntil( () => self.IsState(fullPathHash))と同値 
	/// </summary>
	public static IEnumerator WaitToTransitTo(this Animator self, int fullPathHash, int layerNo = 0)
	{
		yield return new WaitUntil(() => self.IsState(fullPathHash, layerNo));
	}
}
