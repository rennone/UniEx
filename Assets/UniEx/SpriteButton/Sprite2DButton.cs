using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Sprite2DButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
	[SerializeField]
	string Clicked = "Clicked";

	[SerializeField]
	string Highlighted = "Highlighted";

	[SerializeField]
	string Normal = "Normal";

	[SerializeField]
	string Disabled = "Disabled";

	// 有効無効
	[SerializeField]
	bool interactable_ = true;
	public bool Interactable
	{
		get { return interactable_; }
		set
		{
			if (interactable_ != value)
				OnChange(value);

			interactable_ = value;
			CachedCollider.isTrigger = value;
		}
	}

	// アニメーター
	[SerializeField]
	Animator animator_;
	Animator Controller { get {	 return animator_; } }

	// クリックイベント
	[SerializeField]
	Button.ButtonClickedEvent onClick_ = new Button.ButtonClickedEvent();
	public Button.ButtonClickedEvent OnClick { get { return onClick_; } }

	// 非Interactable時のクリックイベント
	[SerializeField]
	Button.ButtonClickedEvent onDisabledClick_ = new Button.ButtonClickedEvent();
	public Button.ButtonClickedEvent OnDisabledClick { get { return onDisabledClick_; } }

	// コライダ
	CachedComponent<BoxCollider2D> cachedCollider_ = new CachedComponent<BoxCollider2D>();
	BoxCollider2D CachedCollider { get { return cachedCollider_.GetComponentInChildren(transform); } }

	public bool IsEnable { get { return Interactable && CachedCollider.isTrigger; } }
	public bool IsDisable { get { return !IsEnable; } }

	public bool IsValid { get { return Controller != null && Controller.isInitialized; } }

	// 
	/// <summary>
	/// ポインターハンドラー検出時のチェック関数.
	/// これが null or true を返す時に, クリックイベントなどが有効になる
	/// </summary>
	public Func<PointerEventData, bool> PointHandlerCheck { get; set; }

	// Interactableが変わった時
	void OnChange(bool value)
	{
		if (value)
			OnButtonNormal();
		else
			OnButtonDisable();

		CachedCollider.isTrigger = value;
	}

	void SetTrigger(string name)
	{
		if (IsValid == false)
			return;
		Controller.SetTrigger(name);
	}

	void ResetTrigger(string name)
	{
		if (IsValid == false)
			return;
		Controller.ResetTrigger(name);
	}

	public void OnPointerClick()
	{
		if (IsDisable)
		{
			// UnityのuGUIを確認したところInteractableが切れているときは
			// 他のイベントの時もDisableのトリガーが発火するようになっていたので合わせる
			OnButtonDisable();
			// 非Interactable時にもイベントを発火する
			OnDisabledClick.Invoke();
			return;
		}

		ResetTrigger(Highlighted);
		ResetTrigger(Normal);

		SetTrigger(Clicked);

		// 発火処理
		OnClick.Invoke();
	}

	public void OnPointerEnter()
	{
		if (IsDisable)
		{
			// UnityのuGUIを確認したところInteractableが切れているときは
			// 他のイベントの時もDisableのトリガーが発火するようになっていたので合わせる
			OnButtonDisable();
			return;
		}

		// OnPointExitでの遷移中にこの関数が呼ばれると
		// 予期せぬ挙動(上にあるのにハイライトが消える)を起こすので
		// 予めリセットしておく
		ResetTrigger(Normal);
		ResetTrigger(Clicked);

		// ハイライトのトリガーを付ける
		SetTrigger(Highlighted);
	}

	public void OnPointerExit()
	{
		if (IsDisable)
		{
			// UnityのuGUIを確認したところInteractableが切れているときは
			// 他のイベントの時もDisableのトリガーが発火するようになっていたので合わせる
			OnButtonDisable();
			return;
		}

		// OnPointEnterでの遷移中にこの関数が呼ばれると
		// 予期せぬ挙動(ハイライトが戻らない)を起こすので
		// 予めリセットしておく
		ResetTrigger(Highlighted);
		ResetTrigger(Clicked);

		SetTrigger(Normal);
	}

	void OnButtonDisable()
	{
		ResetTrigger(Highlighted);
		ResetTrigger(Clicked);
		ResetTrigger(Normal);
		SetTrigger(Disabled);
	}

	void OnButtonNormal()
	{
		SetTrigger(Normal);
	}

	void Update()
	{
#if DEBUG
		// InspectorからInteractableをいじった時用
		if (CachedCollider.isTrigger != Interactable)
			Interactable = CachedCollider.isTrigger;
#endif
	}

	void Awake()
	{
		CachedCollider.isTrigger = Interactable;
	}

	void OnEnable()
	{
		if (Interactable == false)
			OnButtonDisable();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (HandlerEnable(eventData))
			OnPointerClick();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (HandlerEnable(eventData))
			OnPointerExit();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (HandlerEnable(eventData))
			OnPointerEnter();
	}

	bool HandlerEnable(PointerEventData eventData)
	{
		return PointHandlerCheck == null || PointHandlerCheck(eventData);
	}
}
