// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utage;
using UtageExtensions;

namespace Utage
{

	// ノベルゲームのメニューボタンの処理
	// 後付けのため、UtageUguiMainGameと機能が重複している点に注意
	// UtageUguiMainGameはノベルゲーム以外での使用が難しかったので、
	// UtageUguiMenuButtonsはノベルゲーム以外でも使用できるように実装する
	[AddComponentMenu("Utage/TemplateUI/UtageUguiMenuButtons")]
	public class UtageUguiMenuButtons : MonoBehaviour
	{
		// ADVエンジン
		public virtual AdvEngine Engine => this.GetComponentCacheFindIfMissing(ref engine);
		[SerializeField] protected AdvEngine engine;

		[SerializeField] protected GameObject rootButtons;
		[SerializeField] protected Image bg;
		
		// スキップボタン
		[SerializeField] Toggle checkSkip;

		// 自動で読み進むボタン
		[SerializeField] Toggle checkAuto;

		//ノベルゲームモードでのメイン画面
		[SerializeField] UtageUguiMainGame mainGame;
		//コンフィグ画面（ノベルゲーム以外で直接開く場合）
		[SerializeField] UtageUguiConfig config;


		public virtual void Open()
		{
			this.gameObject.SetActive(true);
		}

		public virtual void Close()
		{
			this.gameObject.SetActive(false);
		}

		protected virtual void LateUpdate()
		{
			//メニューボタンの表示・表示を切り替え
			bool activeMenuButtons = Engine.UiManager.IsShowingMenuButton && Engine.UiManager.Status == AdvUiManager.UiStatus.Default;

			//メニューボタンの表示・表示を切り替え
			if (rootButtons != null)
			{
				rootButtons.SetActive(activeMenuButtons);
			}

			//メニュー背景の表示・表示を切り替え
			if (bg != null)
			{
				//メニューボタンは表示するが、メッセージウィンドウを非表示にする場合は、メニュー背景を表示にする
				bg.enabled = activeMenuButtons && !Engine.UiManager.IsShowingMessageWindow;
			}

			//スキップフラグを反映
			if (checkSkip)
			{
				if (checkSkip.isOn != Engine.Config.IsSkip)
				{
					checkSkip.isOn = Engine.Config.IsSkip;
				}
			}

			//オートフラグを反映
			if (checkAuto)
			{
				if (checkAuto.isOn != Engine.Config.IsAutoBrPage)
				{
					checkAuto.isOn = Engine.Config.IsAutoBrPage;
				}
			}
		}


		//スキップボタンが押された
		public virtual void OnTapSkip(bool isOn)
		{
			Engine.Config.IsSkip = isOn;
		}

		//自動読み進みボタンが押された
		public virtual void OnTapAuto(bool isOn)
		{
			Engine.Config.IsAutoBrPage = isOn;
		}

		//ウインドウ閉じるボタンが押された
		public virtual void OnTapCloseWindow()
		{
			Engine.UiManager.Status = AdvUiManager.UiStatus.HideMessageWindow;
		}

		//バックログボタンが押された
		public virtual void OnTapBackLog()
		{
			Engine.UiManager.Status = AdvUiManager.UiStatus.Backlog;
		}


		//コンフィグボタンが押された
		public virtual void OnTapConfig()
		{
			if(mainGame!=null)
			{
				mainGame.OnTapConfig();
			}
			else
			{
				config.Open();
			}
		}

	}
}
