// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utage
{

	//「Utage」のシナリオデータ用の文字数をチェックする
	//開いているシーン内にメッセージウィンドウがあれば、その制御をして文字数がオーバーしないかもチェック
	public class AdvScenarioCharacterCountChecker
	{
		AdvEngine Engine { get; set; }
		Dictionary<string, IAdvMessageWindowCaracterCountChecker> Windows = new Dictionary<string, IAdvMessageWindowCaracterCountChecker>();

		// 全シナリオの文字数をカウントしてチェック
		internal bool TryCheckCharacterCount( List<AdvScenarioData> scenarioList, out int count )
		{
			count = 0;
			Engine = UtageEditorToolKit.FindComponentAllInTheScene<AdvEngine>();
			if (Engine == null) return false;

			AdvUiManager uiManager = UtageEditorToolKit.FindComponentAllInTheScene<AdvUiManager>();
			if (uiManager == null) return false;

			bool isActive = uiManager.gameObject.activeSelf;
			if (!isActive)
			{
				uiManager.gameObject.SetActive(true);
			}
			Windows.Clear();
			foreach (var keyValue in Engine.MessageWindowManager.AllWindows)
			{
				var window = keyValue.Value.MessageWindow as IAdvMessageWindowCaracterCountChecker;
				if (window != null)
				{
					Windows.Add(keyValue.Key, window);
				}
			}
			if (Windows.Count == 0)
			{
				Debug.LogWarning("文字数カウント可能なメッセージウィンドがありません");
			}
			else
			{
				Dictionary<IAdvMessageWindowCaracterCountChecker, string> logStrings = new Dictionary<IAdvMessageWindowCaracterCountChecker, string>();
				foreach (var keyValue in Windows)
				{
					logStrings.Add(keyValue.Value, keyValue.Value.StartCheckCaracterCount());
				}
				foreach (AdvScenarioData data in scenarioList)
				{
					count += CheckCharacterCount(data);
				}
				foreach (var keyValue in logStrings)
				{
					keyValue.Key.EndCheckCaracterCount(keyValue.Value);
				}
			}
			if (!isActive) uiManager.gameObject.SetActive(false);
			return true;
		}

		// 1シナリオデータ内の文字数をカウントしてチェック
		int CheckCharacterCount(AdvScenarioData data)
		{
			int count = 0;
			foreach (var keyValue in data.ScenarioLabels)
			{
				count += CheckCharacterCount(keyValue.Value);
			}
			return count;
		}

		// 1シナリオラベルデータ内の文字数をカウントしてチェック
		int CheckCharacterCount(AdvScenarioLabelData data)
		{
			int count = 0;
			string currentWindowName = "";
			foreach (AdvScenarioPageData page in data.PageDataList)
			{
				count += CheckCharacterCount(page, ref currentWindowName);
			}
			return count;
		}

		// 1ページの文字数をカウントしてチェック
		int CheckCharacterCount(AdvScenarioPageData page, ref string currentWindowName)
		{
			IAdvMessageWindowCaracterCountChecker messageWindow;
			if (!string.IsNullOrEmpty(page.MessageWindowName)) currentWindowName = page.MessageWindowName;

			if (!Windows.TryGetValue(currentWindowName, out messageWindow))
			{
				foreach (var window in Windows.Values)
				{
					messageWindow = window;
					break;
				}
			}

			//アクティブオフ状態だったらOnにしておく
			bool isActive = messageWindow.gameObject.activeSelf;
			messageWindow.gameObject.SetActive(true);

			//ローカライズに対応
			UguiLocalizeBase[] localizeArray = messageWindow.gameObject.GetComponentsInChildren<UguiLocalizeBase>();
			foreach (var item in localizeArray)
			{
				item.EditorRefresh();
			}

			//文字数をカウント
			int count = CheckCharacterCount(page, messageWindow);

			//選択肢テキストなどほかのテキストのコマンドをチェック
			CheckOtherTextCommand(page, messageWindow);

			//ローカライズ状態を戻す
			foreach (var item in localizeArray)
			{
				item.ResetDefault();
			}
			//アクテイブ状態をもどす
			messageWindow.gameObject.SetActive(isActive);
			return count;
		}

		// 1ページの文字数をカウントしてチェック
		int CheckCharacterCount(AdvScenarioPageData page, IAdvMessageWindowCaracterCountChecker messageWindow)
		{
			string text = MakeText(page);
			if (text.Length <= 0)
			{
				return 0;
			}
			int count;
			string errorString;
			if (!messageWindow.TryCheckCaracterCount(text, out count, out errorString))
			{
				Debug.LogError("TextOver:" + page.TextDataList[0].RowData.ToStringOfFileSheetLine() + "\n" + errorString);
			}
			return count;
		}

		string MakeText(AdvScenarioPageData page)
		{
			StringBuilder builder = new StringBuilder();
			foreach (var item in page.TextDataList)
			{
				builder.Append(item.ParseCellLocalizedText());
				if (item.IsNextBr) builder.Append("\n");
			}
			return builder.ToString();
		}
		
		//選択肢テキストなどほかのテキストのコマンドをチェック
		void CheckOtherTextCommand(AdvScenarioPageData page, IAdvMessageWindowCaracterCountChecker messageWindow)
		{
			foreach (var item in page.CommandList)
			{
				var selection = item as AdvCommandSelection;
				if (selection != null)
				{
					selection.ParseCellLocalizedText();
				}
			}
		}
	}
}