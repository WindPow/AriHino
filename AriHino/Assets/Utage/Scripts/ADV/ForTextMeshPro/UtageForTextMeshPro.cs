using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using UtageExtensions;

namespace Utage
{
	//宴のTextMeshPro対応
	[AddComponentMenu("Utage/TextMeshPro/UtageForTextMeshPro")]
	public class UtageForTextMeshPro : MonoBehaviour
		, ICustomTextParser
	{
		[SerializeField] bool enableRichTextInParamString = false;
		
		void Awake()
		{
			SetCustomTextParser();
		}

		void OnDestroy()
		{
			ClearCustomTextParser();
		}

		public void SetCustomTextParser()
		{
			//テキスト解析処理をカスタム
			TextData.CreateCustomTextParser = CreateCustomTextParser;
			//ログテキスト作成をカスタム
			TextData.MakeCustomLogText = MakeCustomLogText;
		}

		public void ClearCustomTextParser()
		{
			TextData.CreateCustomTextParser = null;
			TextData.MakeCustomLogText = null;
		}

		//テキスト解析をカスタム
		TextParser CreateCustomTextParser(string text)
		{
			if (enableRichTextInParamString)
			{
				//いったんparamタグだけ変換し、その他のタグは残したままのテキストを生成
				var parseParamOnlyString = new TextParser(text, true).NoneMetaString;
				//改めてテキストの解析処理
				return new TextMeshProTextParser(parseParamOnlyString);
			}
			else
			{
				return new TextMeshProTextParser(text);
			}
		}

		//ログテキスト作成をカスタム
		string MakeCustomLogText(string text)
		{
			return new TextMeshProTextParser(text,true).NoneMetaString;
		}
	}
}
