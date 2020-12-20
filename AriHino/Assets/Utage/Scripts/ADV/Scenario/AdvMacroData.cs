// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtageExtensions;


namespace Utage
{

	/// <summary>
	/// マクロのデータ
	/// </summary>
	public class AdvMacroData
	{
		//マクロ名
		public string Name { get; private set; }
		//マクロのヘッダ部分（マクロ名の行と同じで、引数が入る）
		public StringGridRow Header { get; private set; }
		//マクロ部分のデータ
		public List<StringGridRow> DataList { get; private set; }
		public AdvMacroData(string name, StringGridRow header, List<StringGridRow> dataList)
		{
			this.Name = name;
			this.Header = header;
			this.DataList = dataList;
		}

		//指定の行をマクロ展開
		public List<StringGridRow> MacroExpansion(StringGridRow args, string debugMsg)
		{
			//マクロ展開後の行リスト
			List<StringGridRow> list = new List<StringGridRow>();
			if (DataList.Count <= 0) return list;


			//展開先の列数と同じ数のセル（文字列の配列）をもつ
			int maxStringCount = 0;
			foreach (var keyValue in args.Grid.ColumnIndexTbl)
			{
				maxStringCount = Mathf.Max(keyValue.Value, maxStringCount);
			}
			maxStringCount += 1;
			for (int i = 0; i < DataList.Count; ++i)
			{
				string[] strings = new string[maxStringCount];
				for (int index = 0; index < strings.Length; ++index)
				{
					strings[index] = "";
				}
				StringGridRow data = DataList[i];
				//展開先の列数と同じ数のセル（文字列の配列）をもつ
				foreach (var keyValue in args.Grid.ColumnIndexTbl)
				{
					string argKey = keyValue.Key;
					int argIndex = keyValue.Value;
					strings[argIndex] = ParaseMacroArg(data.ParseCellOptional<string>(argKey, ""), args);
				}
				//展開先のシートの構造に合わせる
				//展開先シートを親Girdに持ち
				StringGridRow macroData = new StringGridRow(args.Grid, args.RowIndex);
				macroData.InitFromStringArray(strings);
				list.Add(macroData);

				//デバッグ情報の記録
				macroData.DebugInfo = debugMsg + " : " + (data.RowIndex + 1) + " ";
			}
			return list;
		}

		//マクロ引数展開
		string ParaseMacroArg(string str, StringGridRow args)
		{
			int index = 0;
			string macroText = "";
			while (index < str.Length)
			{
				bool isFind = false;
				if (str[index] == '%')
				{
					foreach (string key in Header.Grid.ColumnIndexTbl.Keys)
					{
						if (key.Length <= 0) continue;
						for (int i = 0; i < key.Length; ++i)
						{
							if (key[i] != str[index + 1 + i])
							{
								break;
							}
							else if (i == key.Length - 1)
							{
								isFind = true;
							}
						}
						if (isFind)
						{
							string def = Header.ParseCellOptional<string>(key, "");
							macroText += args.ParseCellOptional<string>(key, def);
							index += key.Length;
							break;
						}
					}
				}
				if (!isFind)
				{
					macroText += str[index];
				}
				++index;
			}
			return macroText;
		}


	}
}