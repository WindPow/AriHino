using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// シナリオテキストファイル（.txt）をcsvに整形して変換するツール
/// </summary>
public class ScenarioTextToCsvExport : EditorWindow
{
	// 変換するテキストファイル
	TextAsset textFile;
	// 出力するパス
	string inputPath;

	bool isLink = false;
	
	/// <summary>
	/// メニューにWindowを追加
	/// </summary>
	[MenuItem("Custom/CsvExport")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow<ScenarioTextToCsvExport>("シナリオファイル変換");
	}

	/// <summary>
	/// 処理内容
	/// </summary>
	private void OnGUI()
	{
		// ファイル選択
		using (new GUILayout.VerticalScope(EditorStyles.helpBox))
		{
			
			using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				GUILayout.Label("変換するテキストファイル");
			}

			textFile = EditorGUILayout.ObjectField(textFile, typeof(TextAsset), true) as TextAsset;
		}

		GUILayout.Space(20);

		// 保存パス
		using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
		{
			GUILayout.Label("保存名");
			inputPath = GUILayout.TextField(inputPath);
			GUILayout.Label("パス：" + Application.dataPath + "/Resources/" + inputPath + ".csv");

			if (GUILayout.Button("CSVにエクスポート", GUILayout.Height(30)))
			{
				Export();
			}
		}
	}

	/// <summary>
	/// 出力処理
	/// </summary>
	private void Export()
	{
		// 改行文字で分割したテキスト
		string[] splitTexts = textFile.text.Split('\n');
		// 最終的に追加していくリスト
		List<string> addList = new List<string>();
		// 連結する文字列
		string linkString = string.Empty;
		
		foreach (var splitLine in splitTexts)
		{
			// 文字列がNULLか空白だったらcontinue
			if (string.IsNullOrWhiteSpace(splitLine))
			{
				// 連結フラグがtrueならフラグを消して整形後にリスト追加
				if (isLink)
				{
					addList.Add(LexicalAnalysis('"' + linkString.TrimEnd() + '"' + "\n"));
				}
				isLink = false;
				linkString = null;

				continue;
			}

			// 整形
			string lexical = LexicalAnalysis(splitLine);

			// 連結フラグがtrueなら連結してcontinue
			if (isLink)
			{
				linkString += splitLine;

				continue;
			}

			addList.Add(lexical);
		}

		// リソースフォルダにファイルを生成
		StreamWriter sw;
		FileInfo fi = new FileInfo(Application.dataPath + "/Resources/" + inputPath + ".csv");

		sw = fi.CreateText();

		// 書き込む
		foreach (var line in addList)
		{
			sw.Write(line);
		}

		sw.Flush();
		sw.Close();
	}

	/// <summary>
	/// 字句解析して文字列をCSV形式に変換
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	private string LexicalAnalysis(string str)
	{
		isLink = false;

		if (str.Contains(":"))
		{
			string[] split = str.Split(':');
			return "," + split[0] + "," + "," + "," + "," + "," + "," + "," + split[1];
		}
		if (str.Contains("："))
		{
			string[] split = str.Split('：');
			return "," + split[0] + "," + "," + "," + "," + "," + "," + "," + split[1];
		}
		if (str.Contains("SE") || str.Contains("Se"))
		{
			if (str.Contains("　"))
			{
				string[] split = str.Split('　');
				return "Se" + "," + split[1];
			}
			if (str.Contains(" "))
			{
				string[] split = str.Split(' ');
				return "Se" + "," + split[1];
			}
		}
		if (str.Contains("BGM") || str.Contains("Bgm"))
		{
			if (str.Contains("　"))
			{
				string[] split = str.Split('　');
				return "Bgm" + "," + split[1];
			}
			if (str.Contains(" "))
			{
				string[] split = str.Split(' ');
				return "Bgm" + "," + split[1];
			}
		}
		if (str.Contains("フェードイン"))
		{
			return "FadeIn";
		}
		if (str.Contains("フェードアウト"))
		{
			return "FadeOut";
		}
		if (str.Contains("Wait"))
		{
			string[] split = str.Split('　');
			return "Wait" + "," + "," + "," + "," + "," + split[1];
		}
		if (str.Contains("Character消"))
		{
			return "CharacterOff";
		}

		// 解析に引っかからない（通常分）場合連結フラグON
		isLink = true;
		return "," + "," + "," + "," + "," + "," + "," + "," + str;
	}
}
