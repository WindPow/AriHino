// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utage
{

	//「Utage」のシナリオデータ用のエクセルファイルインポーター
	public class AdvExcelImporter : AssetPostprocessor
	{
		static void OnPostprocessAllAssets(
			string[] importedAssets,
			string[] deletedAssets,
			string[] movedAssets,
			string[] movedFromAssetPaths)
		{
			//制御エディタを通して、管理対象のデータのみインポートする
			AdvScenarioDataBuilderWindow.Import(importedAssets);
		}
		public const string BookAssetExt = ".book.asset";
		public const string ChapterAssetExt = ".chapter.asset";
		public const string ScenarioAssetExt = ".asset";

		//シナリオデータ
		Dictionary<string, AdvScenarioData> scenarioDataTbl;
		AdvMacroManager macroManager;
		AdvImportScenarios scenariosAsset;

		AdvScenarioDataProject Project { get; set; }
		bool allImport = false;
		List<string> importedAssets = null;

		//ファイルの読み込み
		public void ImportAll(AdvScenarioDataProject project)
		{
			this.allImport = true;
			ImportSub(project);
		}
		//ファイルの読み込み
		public void Import(AdvScenarioDataProject project, string[] importedAssets)
		{
			this.allImport = false;
			this.importedAssets = new List<string>(importedAssets);
			ImportSub(project);
		}

		//ファイルの読み込み
		void ImportSub(AdvScenarioDataProject project)
		{
			Project = project;
			if (project.ChapterDataList.Count <= 0)
			{
				Debug.LogError("ChapterDataList is zeo");
				return;
			}

			AssetFileManager.ClearCheckErrorInEditor();
			AssetFileManager.IsEditorErrorCheck = true;
			AdvCommand.IsEditorErrorCheck = true;
			AdvCommand.IsEditorErrorCheckWaitType = project.CheckWaitType;

			UnityEngine.Profiling.Profiler.BeginSample("Import Scenarios");
			AdvEngine engine = UtageEditorToolKit.FindComponentAllInTheScene<AdvEngine>();
			if (engine != null)
			{
				engine.BootInitCustomCommand();
			}
			this.scenarioDataTbl = new Dictionary<string, AdvScenarioData>();
			this.macroManager = new AdvMacroManager();

			AdvScenarioDataBuilderWindow.ProjectData.CreateScenariosIfMissing();
			this.scenariosAsset = project.Scenarios;

			this.scenariosAsset.ClearOnImport();
			//チャプターデータのインポート
			project.ChapterDataList.ForEach(x => ImportChapter(x.chapterName, x.ExcelPathList));

			//ファイルが存在しているかチェック
			if (project.RecourceDir != null)
			{
				string path = new MainAssetInfo(project.RecourceDir).FullPath;
				AssetFileManager.CheckErrorInEditor(path, project.CheckExt);
			}
			UnityEngine.Profiling.Profiler.EndSample();



			EditorUtility.SetDirty(this.scenariosAsset);
			AssetDatabase.Refresh();
			AdvCommand.IsEditorErrorCheck = false;
			AdvCommand.IsEditorErrorCheckWaitType = false;
			AssetFileManager.IsEditorErrorCheck = false;

		}

		bool ImportChapter(string chapterName, List<string> pathList)
		{
			if (pathList.Count <= 0) return false;

			List<AdvImportBook> bookAssetList = new List<AdvImportBook>();

			//エクセルファイルのアセットを取得
			foreach (var path in pathList)
			{
				if (string.IsNullOrEmpty(path)) continue;

				AdvImportBook bookAsset;
				//再インポートが必要なアセットを取得
				//失敗する→再インポートが必要なし
				if (CheckReimport(path, out bookAsset))
				{
					Debug.Log("Reimport " + path);
					//対象のエクセルファイルを読み込み
					StringGridDictionary book = ReadExcel(path);
					if (book.List.Count <= 0)
					{
						//中身がない
						continue;
					}
					//末尾の空白文字をチェック
					if (Project.CheckWhiteSpaceEndOfCell) CheckWhiteSpaceEndOfCell(book);
					bookAsset.Clear();
					bookAsset.AddSrourceBook(book);
				}
				bookAssetList.Add(bookAsset);
			}
			//インポート処理をする
			ImportChapter(chapterName, bookAssetList);

			//変更を反映
			foreach (var asset in bookAssetList)
			{
				Debug.Log(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.Import, asset.name));
				EditorUtility.SetDirty(asset);
			}
			return true;
		}
		//対象のエクセルファイルを全て読み込み
		StringGridDictionary ReadExcel(string path)
		{
			StringGridDictionary book = ExcelParser.Read(path, '#', Project.ParseFormula, Project.ParseNumreic);
			book.RemoveSheets(@"^#");
			if (Project.EnableCommentOutOnImport)
			{
				book.EraseCommentOutStrings(@"//");
			}
			return book;
		}


		//再インポートが必要なアセットを取得
		bool CheckReimport(string path, out AdvImportBook bookAsset)
		{
			//シナリオデータ用のスクリプタブルオブジェクトを宣言
			string bookAssetPath = Path.ChangeExtension(path, BookAssetExt);
			bookAsset = AssetDatabase.LoadAssetAtPath<AdvImportBook>(bookAssetPath);
			if (bookAsset == null)
			{
				//まだないので作る
				bookAsset = ScriptableObject.CreateInstance<AdvImportBook>();
				AssetDatabase.CreateAsset(bookAsset, bookAssetPath);
				bookAsset.hideFlags = HideFlags.NotEditable;
				return true;
			}
			else
			{
				return CheckReimportFromPath(path);
			}
		}

		//再インポートが必要かパスからチェック
		bool CheckReimportFromPath(string path)
		{
			if (allImport) return true;
			return importedAssets.Contains(path);
		}

		//末尾の空白文字をチェック
		private void CheckWhiteSpaceEndOfCell(StringGridDictionary book)
		{
			AdvEditorSettingWindow editorSetting = AdvEditorSettingWindow.GetInstance();
			if (UnityEngine.Object.ReferenceEquals(editorSetting, null)) return;
			if (!editorSetting.CheckWhiteSpaceOnImport) return;

			List<string> ignoreHeader = new List<string>();
			ignoreHeader.Add("Text");
			if (LanguageManagerBase.Instance != null)
			{
				foreach (string language in LanguageManagerBase.Instance.Languages)
				{
					ignoreHeader.Add(language);
				}
			}

			foreach (var sheet in book.Values)
			{
				List<int> ignoreIndex = new List<int>();
				foreach (var item in ignoreHeader)
				{
					int index;
					if (sheet.Grid.TryGetColumnIndex(item, out index))
					{
						ignoreIndex.Add(index);
					}
				}
				foreach (var row in sheet.Grid.Rows)
				{
					if (row.RowIndex == 0) continue;

					for (int i = 0; i < row.Strings.Length; ++i)
					{
						string str = row.Strings[i];
						if (str.Length <= 0) continue;
						if (ignoreIndex.Contains(i)) continue;

						int endIndex = str.Length - 1;
						if (char.IsWhiteSpace(str[endIndex]))
						{
							Debug.LogWarning(row.ToErrorString("Last characer is white space [" + ColorUtil.AddColorTag(str, ColorUtil.Red) + "]  \n"));
						}
					}
				}
			}
		}

		//マクロ処理したインポートデータを作成する
		void ImportChapter(string chapterName, List<AdvImportBook> books)
		{
			//チャプターデータを作成し、各シナリオを設定
			string path = AssetDatabase.GetAssetPath(this.Project);
			path = FilePathUtil.Combine(FilePathUtil.GetDirectoryPath(path), chapterName);
			AdvChapterData chapter = LoadOrCreateChapterAsset(path);
			this.scenariosAsset.AddChapter(chapter);

			//初期化
			chapter.ImportBooks(books, this.macroManager);

			//設定データの解析とインポート
			AdvSettingDataManager setting = new AdvSettingDataManager();
			setting.ImportedScenarios = this.scenariosAsset;
			setting.BootInit("");
			chapter.MakeScenarioImportData(setting, this.macroManager);
			EditorUtility.SetDirty(chapter);
			AdvGraphicInfo.CallbackExpression = setting.DefaultParam.CalcExpressionBoolean;
			TextParser.CallbackCalcExpression = setting.DefaultParam.CalcExpressionNotSetParam;
			iTweenData.CallbackGetValue = setting.DefaultParam.GetParameter;

			List<AdvScenarioData> scenarioList = new List<AdvScenarioData>();
			foreach (var book in books)
			{
				foreach (var grid in book.ImportGridList)
				{
					grid.InitLink();
					string sheetName = grid.SheetName;
					if (!AdvSheetParser.IsScenarioSheet(sheetName)) continue;
					if (scenarioDataTbl.ContainsKey(sheetName))
					{
						Debug.LogError(sheetName + " is already contains in the sheets");
					}
					else
					{
						AdvScenarioData scenario = new AdvScenarioData(grid);
						scenarioDataTbl.Add(sheetName, scenario);
						scenarioList.Add(scenario);
					}
				}
			}

			//シナリオデータとして解析、初期化
			foreach (AdvScenarioData data in scenarioList)
			{
				data.Init(setting);
			}


			AdvGraphicInfo.CallbackExpression = null;
			TextParser.CallbackCalcExpression = null;
			iTweenData.CallbackGetValue = null;

			//シナリオラベルのリンクチェック
			ErrorCheckScenarioLabel(scenarioList);

			//文字数カウント
			if (Project.CheckTextCountAllLanguage)
			{
				var langManager = LanguageManagerBase.Instance;
				if (langManager != null)
				{
					string defLanguage = langManager.CurrentLanguage;
					foreach(var language in langManager.Languages )
					{
						langManager.CurrentLanguage = language;
						CheckCharacterCount(scenarioList, language);
					}
					langManager.CurrentLanguage = defLanguage;
				}
			}
			else if (Project.CheckTextCount)
			{
				CheckCharacterCount(scenarioList,"");
			}
		}

		void CheckCharacterCount(List<AdvScenarioData> scenarioList, string language)
		{
			AdvScenarioCharacterCountChecker checker = new AdvScenarioCharacterCountChecker();
			int count;
			if (checker.TryCheckCharacterCount(scenarioList, out count))
			{
				if (string.IsNullOrEmpty(language))
				{
					Debug.Log(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.ChacacterCountOnImport, count));
				}
				else
				{
					Debug.Log(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.ChacacterCountOnImport, count) + "  in " + language);
				}
			}
		}


		//チャプターデータのアセット取得
		AdvChapterData LoadOrCreateChapterAsset( string path)
		{
			string assetPath = Path.ChangeExtension(path, ChapterAssetExt);
			AdvChapterData asset = AssetDatabase.LoadAssetAtPath<AdvChapterData>(assetPath);
			if (asset == null)
			{
				//まだないので作る
				asset = ScriptableObject.CreateInstance<AdvChapterData>();
				AssetDatabase.CreateAsset(asset, assetPath);
				asset.hideFlags = HideFlags.NotEditable;
			}
			return asset;
		}

		/// <summary>
		/// シナリオラベルのリンクチェック
		/// </summary>
		/// <param name="label">シナリオラベル</param>
		/// <returns>あればtrue。なければfalse</returns>
		void ErrorCheckScenarioLabel(List<AdvScenarioData> scenarioList)
		{
			//リンク先のシナリオラベルがあるかチェック
			foreach (AdvScenarioData data in scenarioList)
			{
				foreach (AdvScenarioJumpData jumpData in data.JumpDataList)
				{
					if (!IsExistScenarioLabel(jumpData.ToLabel))
					{
						Debug.LogError( 
							jumpData.FromRow.ToErrorString( 
							LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotLinkedScenarioLabel, jumpData.ToLabel, "")
							));
					}
				}
			}

			//シナリオラベルが重複しているかチェック
			foreach (AdvScenarioData data in scenarioList)
			{
				foreach (var keyValue in data.ScenarioLabels)
				{
					AdvScenarioLabelData labelData = keyValue.Value;
					if (IsExistScenarioLabel(labelData.ScenarioLabel, data))
					{
						string error = labelData.ToErrorString(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.RedefinitionScenarioLabel, labelData.ScenarioLabel,""), data.DataGridName );
						Debug.LogError(error);
					}
				}
			}
		}


		/// <summary>
		/// シナリオラベルがあるかチェック
		/// </summary>
		/// <param name="label">シナリオラベル</param>
		/// <param name="egnoreData">チェックを無視するデータ</param>
		/// <returns>あればtrue。なければfalse</returns>
		bool IsExistScenarioLabel(string label, AdvScenarioData egnoreData = null )
		{
			foreach (AdvScenarioData data in scenarioDataTbl.Values)
			{
				if (data == egnoreData) continue;
				if (data.IsContainsScenarioLabel(label))
				{
					return true;
				}
			}
			return false;
		}
	}
}