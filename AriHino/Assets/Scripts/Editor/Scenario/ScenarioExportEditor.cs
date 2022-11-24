using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System;
using System.Text;
using UniRx;
using System.Linq;

namespace Arihino.Editor
{
    /// <summary>
    /// 本稿用シナリオファイル（Csv）を読み込んで実データファイルを出力する拡張ツール
    /// </summary>
    public class ScenarioExportEditor : EditorWindow
    {
        // 読み込むCSVファイルパス
        private string inputCsvPath;
        // 出力先xlsxファイルパス
        private string outPutPath;
        // 読み込むファイル
        private ReactiveProperty<TextAsset> inputFile = new ReactiveProperty<TextAsset>();
        // 出力先ファイル
        private ReactiveProperty<DefaultAsset> outputFile = new ReactiveProperty<DefaultAsset>();

        // シナリオデータクラスの辞書
        private Dictionary<int, ScenarioExportData> scenarioDic = new Dictionary<int, ScenarioExportData>();
       
        // 出力先エクセルファイルのシート番号
        private int outputSheetNum;
        // 読み込み開始No
        private int hedNo = 1;
        // 読み込み終了No
        private int endNo = 1;
        // 全てのデータを読み込むか
        private bool isAllInput;

        [MenuItem("Custom/ScenarioExportEditor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ScenarioExportEditor>("シナリオファイル変換");
        }

        // Start is called before the first frame update
        void Awake()
        {
            inputFile.Subscribe(e =>
            {
                ChangedInputFile(e);
            });

            outputFile.Subscribe(e =>
            {
                ChangedOutputFile(e);
            });                        
        }

        // 読み込むファイル変更時の処理
        private void ChangedInputFile(TextAsset inputFile) {

            inputCsvPath = AssetDatabase.GetAssetPath(inputFile);
        }

        /// <summary>
        /// 出力先ファイル変更時の処理
        /// </summary>
        private void ChangedOutputFile(DefaultAsset outputFile) {

            outPutPath = AssetDatabase.GetAssetPath(outputFile);
        }

        private void OnGUI()
        {
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
				{
					GUILayout.Label("変換するCsvファイルパス");
				}

                inputFile.Value = EditorGUILayout.ObjectField(inputFile.Value, typeof(TextAsset), true) as TextAsset;      
			}

			EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("出力先エクセルファイル");
                }

                outputFile.Value = EditorGUILayout.ObjectField(outputFile.Value, typeof(DefaultAsset), true) as DefaultAsset;
            }

            EditorGUILayout.Space();

            if (inputCsvPath == null) return;           

            isAllInput = EditorGUILayout.Toggle("全てのデータを読み込む", isAllInput);

            EditorGUI.BeginDisabledGroup(isAllInput);

            GUILayout.Label("先頭No");
            hedNo = EditorGUILayout.IntField(hedNo);

            if (hedNo < 1) hedNo = 1;

            GUILayout.Label("終末No");
            endNo = EditorGUILayout.IntField(endNo);

            if (endNo < hedNo) endNo = hedNo;

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            GUILayout.Label("シート番号");
            outputSheetNum = EditorGUILayout.IntField(outputSheetNum);

            EditorGUILayout.Space();

            if (GUILayout.Button("Csvを読み込んでシナリオを出力"))
            {
                // 出力開始
                Export();
            }
        }

        /// <summary>
        /// 出力
        /// </summary>
        private void Export()
        {
            ReadInputScenarioFile(inputCsvPath);

            ExportScenarioFile();
        }

        /// <summary>
        /// シナリオCSVファイルを読み込んでデータクラスに格納
        /// </summary>
        /// <param name="fileName"></param>
        private void ReadInputScenarioFile(string fileName)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                NewLine = Environment.NewLine,
                IgnoreBlankLines = true,
                Encoding = Encoding.UTF8,
                AllowComments = true,
                Comment = '#',
                DetectColumnCountChanges = true,
                TrimOptions = TrimOptions.Trim,
            };

            using (var reader = new StreamReader(fileName, Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<ScenarioExportData>();

                foreach (var record in records)
                {
                    scenarioDic[record.No] = record;
                }
            }
        }
        /// <summary>
        /// 出力先ファイルにデータを上書き
        /// </summary>
        private void ExportScenarioFile()
        {
            Excel xls = ExcelHelper.LoadExcel(outPutPath);
            xls.ShowLog();

            foreach (var scenario in scenarioDic)
            {
                if (!xls.Tables.Any() || xls.Tables.Count < outputSheetNum) break;

                if (isAllInput)
                {
                    hedNo = 1;
                    endNo = scenarioDic.Count;
                }
                // 先頭Noより前なら飛ばす
                if (scenario.Key < hedNo) continue;
                //  終末Noより後なら抜ける
                if (scenario.Key > endNo) break;

                int rowNum = scenario.Key + 1;
                xls.Tables[outputSheetNum].SetValue(rowNum, (int)APP_DEFINE.Editor.ScenarioInputDataHedder.Arg1, scenario.Value.CharaName);
                xls.Tables[outputSheetNum].SetValue(rowNum, (int)APP_DEFINE.Editor.ScenarioInputDataHedder.Arg2, scenario.Value.CharaAvatarName);
                xls.Tables[outputSheetNum].SetValue(rowNum, (int)APP_DEFINE.Editor.ScenarioInputDataHedder.Text, scenario.Value.Text);
                xls.Tables[outputSheetNum].SetValue(rowNum, (int)APP_DEFINE.Editor.ScenarioInputDataHedder.ToWrite, scenario.Value.ToWriteId.ToString());
                xls.Tables[outputSheetNum].SetValue(rowNum, (int)APP_DEFINE.Editor.ScenarioInputDataHedder.Discription, scenario.Value.Description);
            }

            ExcelHelper.SaveExcel(xls, outPutPath);
        }
    }

}

