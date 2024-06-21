using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utage;

public class CustomAdvUguiManager : AdvUguiManager
{
	[SerializeField]
	private Toggle hideButtonToggle;

    protected override void Update()
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
						hideButtonToggle.isOn = false;
						//Status = UiStatus.Default;
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
						{
							// Books表示状態の場合Booksを閉じる
							if(BooksManager.Instance.IsOpenBooks) {
								BooksManager.Instance.ActivateBooks(false);
								return;
							}

							hideButtonToggle.isOn = true;

							//右クリックでウィンドウ閉じる
							//Status = UiStatus.HideMessageWindow;
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
}
