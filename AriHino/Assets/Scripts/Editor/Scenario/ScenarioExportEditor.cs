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
    /// �{�e�p�V�i���I�t�@�C���iCsv�j��ǂݍ���Ŏ��f�[�^�t�@�C�����o�͂���g���c�[��
    /// </summary>
    public class ScenarioExportEditor : EditorWindow
    {
        // �ǂݍ���CSV�t�@�C���p�X
        private string inputCsvPath;
        // �o�͐�xlsx�t�@�C���p�X
        private string outPutPath;
        // �ǂݍ��ރt�@�C��
        private ReactiveProperty<TextAsset> inputFile = new ReactiveProperty<TextAsset>();
        // �o�͐�t�@�C��
        private ReactiveProperty<DefaultAsset> outputFile = new ReactiveProperty<DefaultAsset>();

        // �V�i���I�f�[�^�N���X�̎���
        private Dictionary<int, ScenarioExportData> scenarioDic = new Dictionary<int, ScenarioExportData>();
       
        // �o�͐�G�N�Z���t�@�C���̃V�[�g�ԍ�
        private int outputSheetNum;
        // �ǂݍ��݊J�nNo
        private int hedNo = 1;
        // �ǂݍ��ݏI��No
        private int endNo = 1;
        // �S�Ẵf�[�^��ǂݍ��ނ�
        private bool isAllInput;

        [MenuItem("Custom/ScenarioExportEditor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ScenarioExportEditor>("�V�i���I�t�@�C���ϊ�");
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

        // �ǂݍ��ރt�@�C���ύX���̏���
        private void ChangedInputFile(TextAsset inputFile) {

            inputCsvPath = AssetDatabase.GetAssetPath(inputFile);
        }

        /// <summary>
        /// �o�͐�t�@�C���ύX���̏���
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
					GUILayout.Label("�ϊ�����Csv�t�@�C���p�X");
				}

                inputFile.Value = EditorGUILayout.ObjectField(inputFile.Value, typeof(TextAsset), true) as TextAsset;      
			}

			EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("�o�͐�G�N�Z���t�@�C��");
                }

                outputFile.Value = EditorGUILayout.ObjectField(outputFile.Value, typeof(DefaultAsset), true) as DefaultAsset;
            }

            EditorGUILayout.Space();

            if (inputCsvPath == null) return;           

            isAllInput = EditorGUILayout.Toggle("�S�Ẵf�[�^��ǂݍ���", isAllInput);

            EditorGUI.BeginDisabledGroup(isAllInput);

            GUILayout.Label("�擪No");
            hedNo = EditorGUILayout.IntField(hedNo);

            if (hedNo < 1) hedNo = 1;

            GUILayout.Label("�I��No");
            endNo = EditorGUILayout.IntField(endNo);

            if (endNo < hedNo) endNo = hedNo;

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            GUILayout.Label("�V�[�g�ԍ�");
            outputSheetNum = EditorGUILayout.IntField(outputSheetNum);

            EditorGUILayout.Space();

            if (GUILayout.Button("Csv��ǂݍ���ŃV�i���I���o��"))
            {
                // �o�͊J�n
                Export();
            }
        }

        /// <summary>
        /// �o��
        /// </summary>
        private void Export()
        {
            ReadInputScenarioFile(inputCsvPath);

            ExportScenarioFile();
        }

        /// <summary>
        /// �V�i���ICSV�t�@�C����ǂݍ���Ńf�[�^�N���X�Ɋi�[
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
        /// �o�͐�t�@�C���Ƀf�[�^���㏑��
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
                // �擪No���O�Ȃ��΂�
                if (scenario.Key < hedNo) continue;
                //  �I��No����Ȃ甲����
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

