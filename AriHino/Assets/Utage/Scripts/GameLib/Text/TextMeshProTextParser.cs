using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using UtageExtensions;

namespace Utage
{
	//宴のタグ構文解析とTextMeshProのタグ構文解析をして、
	//TextMeshPro形式のタグ付き文字列を生成するためのクラス
	public class TextMeshProTextParser : TextParser
	{
		protected bool NoParse { get; set; }

		//サロゲートペア文字の解析をするかどうか
		protected override bool EnableParseSurrogatePair => true;

		public TextMeshProTextParser(string text, bool isParseParamOnly = false)
			: base(text, isParseParamOnly)
		{
		}

		//TextMeshPro対応のタグ解析
		protected override bool ParseNovelTag(string name, string arg)
		{
			if (string.IsNullOrEmpty(name)) return false;

			//タグの解析を無視するタグが設定されている
			if (NoParse)
			{
				return IsNoParseTag(name);
			}

			//特に処理の必要のTextMeshProの基本タグ
			if (IsDefaultTag(name))
			{
				return true;
			}
			
			//タグ処理が必要なものは処理をする
			if (TryTagOperation(name, arg))
			{
				return true;
			}

			//宴独自のタグの解析
			return base.ParseNovelTag(name, arg);
		}

		//タグの解析を無視するタグが設定されている
		protected virtual bool IsNoParseTag(string name)
		{
			if (name == "/noparse")
			{
				NoParse = false;
				return true;
			}
			else
			{
				return false;
			}
		}

		//特に処理の必要のない基本タグか
		protected virtual bool IsDefaultTag(string name)
		{
			//特に処理の必要のないTextMeshProの基本タグ
			if (defaultTagTbl.Contains(name))
			{
				return true;
			}
			return false;
		}

		//特に処理の必要のないタグ
		static readonly List<string> defaultTagTbl = new List<string>()
			{
				//テキストメッシュプロのタグ
				"align","/align",
				"alpha",//"/alpha",
				"color","/color",
				"b","/b",
				"i","/i",
				"cspace","/cspace",
				"font","/font",
				"indent","/indent",
				"line-height","/line-height",
				"line-indent","/line-indent",
				"link","/link",
				"lowercase","/lowercase",
				"uppercase","/uppercase",
				"smallcaps","/smallcaps",
				"margin","/margin",
				"margin-left","/margin-left",
				"margin-right","/margin-right",
				"mark","/mark",
				"mspace","/mspace",
				"nobr","/nobr",
				"page","/page",
				"pos",
				"size","/size",
				"space",
				"s","/s",
				"u","/u",
				"style","/style",
				"sub","/sub",
				"sup","/sup",
				"voffset","/voffset",
				"width","/width",
				"gradient","/gradient",
			};

		//処理の必要のあるタグなら、処理をする
		protected virtual bool TryTagOperation(string name, string arg)
		{
			//カラーコードのみ指定のタグ
			if (name[0] == '#')
			{
				return true;
			}

			//特別な処理が必要なもの
			switch (name)
			{
				case "noparse":
					NoParse = true;
					return true;
				case "sprite":
					TryAddEmoji(arg);
					return true;
				case "sprite name":
					TryAddEmoji(arg);
					return true;
				case "sprite index":
					TryAddEmoji(arg);
					return true;
				case "dash":
					//二文字ぶん
					AddDash(arg);
					AddDash(arg);
					return true;
				default:
					break;
			}

			return false;
		}

		//タグを作成（特殊な処理が必要な場合はここをoverride）
		protected override TagData MakeTag(string fullString, string name, string arg)
		{
			switch (name)
			{
				//タグの名や引数を書き換える必要があるもの
				case "color":
					{
						string colorString = ToTextMeshProColorString(arg);
						return new TagData("<color=" + colorString + ">", name, colorString);
					}
				case "group":
					return new TagData("<nobr>", "nobr", arg);
				case "/group":
					return new TagData("</nobr>", "nobr", arg);
				case "strike":
					return new TagData("<s>", "s", arg);
				case "/strike":
					return new TagData("</s>", "/s", arg);
				case "emoji":
					return new TagData("<sprite name=\"" + arg+ "\">", "sprite", arg){CountCharacter = 1};

				case "dash":
					//return new TagData("<nobr><mspace=0>--</mspace></nobr>", name, arg);
					int space = int.Parse(arg);
					return new TagData("<nobr><space=0.25em><s> <space=" + (space-1) + "em> </s><space=-0.25em></nobr>", name, arg)
						{ CountCharacter = 1 };
				//宴の独自タグなので、TextMeshProのタグとして表記せずに無視する
				case "ruby":
				case "/ruby":
				case "em":
				case "/em":
//				case "link":
//				case "/link":
				case "tips":
				case "/tips":
				case "sound":
				case "/sound":
				case "speed":
				case "/speed":
				case "interval":
				case "param":
				case "format":
				case "skip_page":
				case "skip_text":
					return new TagData(fullString, name, arg,true);

				//通常のタグ
				default:
					return new TagData(fullString, name, arg);
			}
		}

		//TextMeshPro形式のカラー文字列に変換する
		protected virtual string ToTextMeshProColorString(string color)
		{
			switch (color)
			{
				//TextMeshProがサポートしていないカラー名の場合はカラーコードに変換
				case "aqua": return "#00ffff";
				case "brown": return "#a52a2a";
				case "cyan": return "#00ffff";
				case "darkblue": return "#0000a0";
				case "fuchsia": return "#ff00ff";
				case "grey": return "#808080";
				case "lightblue": return "#add8e6";
				case "lime": return "#00ff00";
				case "magenta": return "#ff00ff";
				case "maroon": return "#800000";
				case "navy": return "#000080";
				case "olive": return "#808000";
				case "silver": return "#c0c0c0";
				case "teal": return "#008080";

				//以下はTextMeshProでもサポートしているカラー名ならそのままで
				case "black":
				case "blue":
				case "green":
				case "orange":
				case "purple":
				case "red":
				case "white":
				case "yellow":
					return color;
				//カラーコードなので変換無し
				default:
					return color;
			}
		}
	}
}
