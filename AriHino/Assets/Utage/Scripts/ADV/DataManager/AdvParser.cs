// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// ADVデータ解析
	/// </summary>
	public class AdvParser
	{
		public static string Localize(AdvColumnName name)
		{
			//多言語化をしてみたけど、複雑になってかえって使いづらそうなのでやめた
			return name.QuickToString();
		}

		//指定の名前のセルを、型Tとして解析・取得（データがなかったらエラーメッセージを出す）
		public static T ParseCell<T>(StringGridRow row, AdvColumnName name)
		{
			return row.ParseCell<T>(Localize(name));
		}

		//指定の名前のセルを、型Tとして解析・取得（データがなかったらデフォルト値を返す）
		public static T ParseCellOptional<T>(StringGridRow row, AdvColumnName name, T defaultVal)
		{
			return row.ParseCellOptional<T>(Localize(name), defaultVal);
		}

		//指定の名前のセルを、型Tとして解析・取得（データがなかったらfalse）
		public static bool TryParseCell<T>(StringGridRow row, AdvColumnName name, out T val)
		{
			return row.TryParseCell<T>(Localize(name), out val);
		}

		//セルが空かどうか
		public static bool IsEmptyCell(StringGridRow row, AdvColumnName name)
		{
			return row.IsEmptyCell(Localize(name));
		}

		//ローカライズも含めてテキスト系コマンドデータが空かどうか
		public static bool IsEmptyTextCommand(StringGridRow row)
		{
			if (!IsEmptyCell(row, AdvColumnName.PageCtrl) || !IsEmptyCell(row, AdvColumnName.Text))
			{
				return false;
			}
			LanguageManagerBase languageManager = LanguageManagerBase.Instance;  
			if (languageManager == null) return true;
			return languageManager.IsEmptyTextCommand(row);
		}

		
		//現在の設定言語にローカライズされたテキストを取得
		public static string ParseCellLocalizedText(StringGridRow row, AdvColumnName defaultColumnName)
		{
			return ParseCellLocalizedText(row, defaultColumnName.QuickToString());
		}

		//現在の設定言語にローカライズされたテキストを取得
		public static string ParseCellLocalizedText(StringGridRow row, string defaultColumnName)
		{
			LanguageManagerBase languageManager = LanguageManagerBase.Instance;  
			if (languageManager == null) return row.ParseCellOptional<string>(defaultColumnName, "");

			return  languageManager.ParseCellLocalizedText(row, defaultColumnName);
		}
		
/*		
		//現在言語のテキストの列名を取得
		public static string GetTextColumnName(StringGridRow row, string defaultColumnName)
		{
			LanguageManagerBase languageManager = LanguageManagerBase.Instance;  
			if (languageManager == null) return defaultColumnName; 

			string currentLanguage = languageManager.CurrentLanguage;
			if (row.Grid.ContainsColumn(currentLanguage))
			{
				//現在の言語があるなら、その列を
				return currentLanguage;
			}
#if UNITY_EDITOR
			switch (languageManager.BlankTextType)
			{
				case LanguageBlankTextType.AllowBlankText:
				case LanguageBlankTextType.NoBlankText:
					if (languageManager.Languages.Contains(currentLanguage))
					{
						//ローカライズ指定言語なのに、行データがない
						Debug.LogError(row.ToErrorString(currentLanguage + " is empty column. Set localize text"));
					}
					break;
				default:
					break;
			}
#endif

			//「DataLanguage」（Text列の言語指定）がないなら、デフォルト行名をそのまま
			string dataLanguage = languageManager.DataLanguage;
			if (string.IsNullOrEmpty(dataLanguage)) return defaultColumnName;
			
			if (dataLanguage==currentLanguage)
			{
				//「DataLanguage」で言語指定がある場合、Text列は指定言語の場合にのみ表示されるようになります。
				return defaultColumnName;
			}
			else if (!string.IsNullOrEmpty(languageManager.DefaultLanguage))
			{
				//DefaultLanguageの列のテキストが基本の表示テキストとして使用されます。
				return languageManager.DefaultLanguage;
			}
			else
			{
				//DefaultLanguageの列のテキストが空の場合は、やはりText列のテキストを表示
				return defaultColumnName;
			}
		}*/
	}
}
