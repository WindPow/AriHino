// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UtageExtensions;


namespace Utage
{
	/// <summary>
	/// UI全般の入力処理。
	/// 独自のキーボード入力などが必要な場合は
	/// これ（AdvUguiManager）かAdvUiManagerを継承して処理を書きかえること
	/// 3.10.0以降、DefaultExecutionOrder(-1)を設定。Update内で入力処理をするので早めに設定
	/// </summary>
	[DefaultExecutionOrder(-1)]	
	[AddComponentMenu("Utage/ADV/AdvUguiManager")]
	public class AdvUguiManager : AdvUiManager
	{
		// メッセージウィンドウ
		public AdvUguiMessageWindowManager MessageWindow{ get { return Engine.MessageWindowManager.UiMessageWindowManager as AdvUguiMessageWindowManager; }}

		[SerializeField]
		protected AdvUguiSelectionManager selection;

		[SerializeField]
		protected AdvUguiBacklogManager backLog;


		//マウスホイールによるバックログの有効・無効
		public bool DisableMouseWheelBackLog { get { return disableMouseWheelBackLog; } set { disableMouseWheelBackLog = value; } }
		[SerializeField]
		protected bool disableMouseWheelBackLog = false;
		
		
		[Flags]
		public enum InputUtilDisableFilter 
		{
			Update =  0x01 << 0,
			OnInput = 0x01 << 1,
		};
		
		//InputUtilが無効の時に、UpdateやOnInputを無視する
		//宴4から初期値を-1（EveryThing）に設定
		public InputUtilDisableFilter FilterInputUtilDisable { get { return filterInputUtilDisable; } set { filterInputUtilDisable = value; } }
		[EnumFlags,SerializeField]
		protected InputUtilDisableFilter filterInputUtilDisable = (InputUtilDisableFilter)(-1);

		//InputUtilが無効の時の設定されたフィルターをチェック
		protected bool CheckInputUtilDisable(InputUtilDisableFilter flag)
		{
			if (InputUtil.EnableInput) return false;
			return (FilterInputUtilDisable & flag) == flag;
		}


		public override void Open()
		{
			this.gameObject.SetActive(true);
			ChangeStatus(UiStatus.Default);
		}

		public override void Close()
		{
			this.gameObject.SetActive(false);
			MessageWindow.Close();
			if (selection != null) selection.Close();
			if (backLog != null) backLog.Close();
		}

		protected override void ChangeStatus(UiStatus newStatus)
		{
			switch (newStatus)
			{
				case UiStatus.Backlog:
					if (backLog == null) return;

					MessageWindow.Close();
					if (selection != null) selection.Close();
					if (backLog != null) backLog.Open();
					Engine.Config.IsSkip = false;
					break;
				case UiStatus.HideMessageWindow:
					MessageWindow.Close();
					if (selection != null) selection.Close();
					if (backLog != null) backLog.Close();
					Engine.Config.IsSkip = false;
					break;
				case UiStatus.Default:
					MessageWindow.Open();
					if (selection != null) selection.Open();
					if (backLog != null) backLog.Close();
					break;
			}
			this.status = newStatus;
		}

		//ウインドウ閉じるボタンが押された
		protected virtual void OnTapCloseWindow()
		{
			Status = UiStatus.HideMessageWindow;
		}

		protected virtual void Update()
		{
			if(CheckInputUtilDisable(InputUtilDisableFilter.Update)) return;
			
			//読み進みなどの入力
			bool IsInput = (Engine.Config.IsMouseWheelSendMessage && InputUtil.IsInputScrollWheelDown())
								|| InputUtil.IsInputKeyboadReturnDown();
			switch (Status)
			{
				case UiStatus.Backlog:
					break;
				case UiStatus.HideMessageWindow:	//メッセージウィンドウが非表示
					//右クリック
					if (InputUtil.IsMouseRightButtonDown())
					{	//通常画面に復帰
						Status = UiStatus.Default;
					}
					else if (!DisableMouseWheelBackLog && InputUtil.IsInputScrollWheelUp())
					{
						//バックログ開く
						Status = UiStatus.Backlog;
					}
					break;
				case UiStatus.Default:
					if (IsShowingMessageWindow)
					{
						//テキストの更新
						Engine.Page.UpdateText();
					}
					if (IsShowingMessageWindow || Engine.SelectionManager.IsWaitInput)
					{	//入力待ち
						if (InputUtil.IsMouseRightButtonDown())
						{	//右クリックでウィンドウ閉じる
							Status = UiStatus.HideMessageWindow;
						}
						else if (!DisableMouseWheelBackLog && InputUtil.IsInputScrollWheelUp())
						{	//バックログ開く
							Status = UiStatus.Backlog;
						}
						else
						{
							if (IsInput)
							{
								//メッセージ送り
								Engine.Page.InputSendMessage();
								base.IsInputTrig = true;
							}
						}
					}
					else
					{
						if (IsInput)
						{
							base.IsInputTrig = false;
						}
					}
					break;
			}
		}

		/// <summary>
		/// タッチされたとき
		/// </summary>
		public virtual void OnPointerDown(BaseEventData data)
		{
			if (data !=null && data is PointerEventData)
			{
				//左クリック入力のみ
				if((data as PointerEventData).button != PointerEventData.InputButton.Left) return;
			}

			OnInput(data);
		}

		/// <summary>
		/// クリックなどの入力があったとき（キーボード入力による文字送りなどを拡張するときに）
		/// </summary>
		public virtual void OnInput(BaseEventData data = null)
		{
			if(CheckInputUtilDisable(InputUtilDisableFilter.OnInput)) return;

			switch (Status)
			{
				case UiStatus.Backlog:
					break;
				case UiStatus.HideMessageWindow:    //メッセージウィンドウが非表示
					Status = UiStatus.Default;
					break;
				case UiStatus.Default:
					if (Engine.Config.IsSkip)
					{
						//スキップ中ならスキップ解除
						Engine.Config.ToggleSkip();
					}
					else
					{
						if (IsShowingMessageWindow)
						{
							if (!Engine.Config.IsSkip)
							{
								//文字送り
								Engine.Page.InputSendMessage();
							}
						}
						if (data != null && data is PointerEventData)
						{
							base.OnPointerDown(data as PointerEventData);
						}
					}
					break;
			}
		}
	}
}
