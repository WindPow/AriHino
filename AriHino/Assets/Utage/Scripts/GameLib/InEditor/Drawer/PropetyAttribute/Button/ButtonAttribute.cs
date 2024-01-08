// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utage
{

	/// <summary>
	/// [Button]アトリビュート（デコレーター）
	/// ボタンを表示する
	/// </summary>
	public class ButtonAttribute : PropertyAttribute
	{
		// ボタンが押されたとき呼ばれる関数名
		GuiDrawerFunction Function { get; }
		
		//ボタンが無効かの判定関数名
		GuiDrawerFunction DisableFunction { get; }

		// ボタンの表示テキスト
		string Text { get; }

		public ButtonAttribute(string function, string text = "", int order = 0)
			:this(function, "", false, text, order)
		{
		}
		public ButtonAttribute(string function, bool nested, string text = "", int order = 0)
			: this(function, "", nested, text, order)
		{
		}

		public ButtonAttribute(string function, string disableFunction, bool nested, string text = "", int order = 0)
		{
			Function = new GuiDrawerFunction(function, nested);
			DisableFunction = new GuiDrawerFunction(disableFunction, nested);
			Text = text;
			this.order = order;
		}

#if UNITY_EDITOR
		// [Button]を表示するためのプロパティ拡張
		[CustomPropertyDrawer(typeof(ButtonAttribute))]
		class Drawer : PropertyDrawerEx<ButtonAttribute>
		{
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				if (!string.IsNullOrEmpty(Attribute.Text))
				{
					label = new GUIContent(Attribute.Text);
				}

				using (new EditorGUI.DisabledScope(IsDisable(property)))
				{
					if (GUI.Button(EditorGUI.IndentedRect(position), label))
					{
						Helper.CallFunction(property, Attribute.Function);
					}
				}
			}
			
			//ボタンが無効かどうか
			bool IsDisable(SerializedProperty property)
			{
				if (Attribute.DisableFunction.Disable) return false;

				return Helper.CallFunction<bool>(property, Attribute.DisableFunction);
			}

		}
#endif
	}
}
