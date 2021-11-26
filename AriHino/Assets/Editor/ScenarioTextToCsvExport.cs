using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// �V�i���I�e�L�X�g�t�@�C���i.txt�j��csv�ɐ��`���ĕϊ�����c�[��
/// </summary>
public class ScenarioTextToCsvExport : EditorWindow
{
	// �ϊ�����e�L�X�g�t�@�C��
	TextAsset textFile;
	// �o�͂���p�X
	string inputPath;

	bool isLink = false;
	
	/// <summary>
	/// ���j���[��Window��ǉ�
	/// </summary>
	[MenuItem("Custom/CsvExport")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow<ScenarioTextToCsvExport>("�V�i���I�t�@�C���ϊ�");
	}

	/// <summary>
	/// �������e
	/// </summary>
	private void OnGUI()
	{
		// �t�@�C���I��
		using (new GUILayout.VerticalScope(EditorStyles.helpBox))
		{
			
			using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				GUILayout.Label("�ϊ�����e�L�X�g�t�@�C��");
			}

			textFile = EditorGUILayout.ObjectField(textFile, typeof(TextAsset), true) as TextAsset;
		}

		GUILayout.Space(20);

		// �ۑ��p�X
		using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
		{
			GUILayout.Label("�ۑ���");
			inputPath = GUILayout.TextField(inputPath);
			GUILayout.Label("�p�X�F" + Application.dataPath + "/Resources/" + inputPath + ".csv");

			if (GUILayout.Button("CSV�ɃG�N�X�|�[�g", GUILayout.Height(30)))
			{
				Export();
			}
		}
	}

	/// <summary>
	/// �o�͏���
	/// </summary>
	private void Export()
	{
		// ���s�����ŕ��������e�L�X�g
		string[] splitTexts = textFile.text.Split('\n');
		// �ŏI�I�ɒǉ����Ă������X�g
		List<string> addList = new List<string>();
		// �A�����镶����
		string linkString = string.Empty;
		
		foreach (var splitLine in splitTexts)
		{
			// ������NULL���󔒂�������continue
			if (string.IsNullOrWhiteSpace(splitLine))
			{
				// �A���t���O��true�Ȃ�t���O�������Đ��`��Ƀ��X�g�ǉ�
				if (isLink)
				{
					addList.Add(LexicalAnalysis('"' + linkString.TrimEnd() + '"' + "\n"));
				}
				isLink = false;
				linkString = null;

				continue;
			}

			// ���`
			string lexical = LexicalAnalysis(splitLine);

			// �A���t���O��true�Ȃ�A������continue
			if (isLink)
			{
				linkString += splitLine;

				continue;
			}

			addList.Add(lexical);
		}

		// ���\�[�X�t�H���_�Ƀt�@�C���𐶐�
		StreamWriter sw;
		FileInfo fi = new FileInfo(Application.dataPath + "/Resources/" + inputPath + ".csv");

		sw = fi.CreateText();

		// ��������
		foreach (var line in addList)
		{
			sw.Write(line);
		}

		sw.Flush();
		sw.Close();
	}

	/// <summary>
	/// �����͂��ĕ������CSV�`���ɕϊ�
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
		if (str.Contains("�F"))
		{
			string[] split = str.Split('�F');
			return "," + split[0] + "," + "," + "," + "," + "," + "," + "," + split[1];
		}
		if (str.Contains("SE") || str.Contains("Se"))
		{
			if (str.Contains("�@"))
			{
				string[] split = str.Split('�@');
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
			if (str.Contains("�@"))
			{
				string[] split = str.Split('�@');
				return "Bgm" + "," + split[1];
			}
			if (str.Contains(" "))
			{
				string[] split = str.Split(' ');
				return "Bgm" + "," + split[1];
			}
		}
		if (str.Contains("�t�F�[�h�C��"))
		{
			return "FadeIn";
		}
		if (str.Contains("�t�F�[�h�A�E�g"))
		{
			return "FadeOut";
		}
		if (str.Contains("Wait"))
		{
			string[] split = str.Split('�@');
			return "Wait" + "," + "," + "," + "," + "," + split[1];
		}
		if (str.Contains("Character��"))
		{
			return "CharacterOff";
		}

		// ��͂Ɉ���������Ȃ��i�ʏ핪�j�ꍇ�A���t���OON
		isLink = true;
		return "," + "," + "," + "," + "," + "," + "," + "," + str;
	}
}
