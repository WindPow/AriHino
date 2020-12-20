// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using System;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// 入力処理
	/// </summary>
	public static class InputUtil
	{
		//入力の有効・無効
		public static bool EnableInput { get { return enableInput; } set { enableInput = value; } }
		static bool enableInput = true;

#if UNITY_2019_3_OR_NEWER
		//Domain Reloadingの対応
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void OnRuntimeInitialize()
		{
			EnableInput = true;
		}
#endif

		[System.Obsolete("Use IsMouseRightButtonDown instead")]
		public static bool IsMousceRightButtonDown()
		{
			if (!EnableInput) return false;
			return IsMouseRightButtonDown();
		}

		public static bool EnableWebGLInput()
		{
#if UTAGE_DISABLE_WEBGL_INPUT
			return falase;
#else
			return (Application.platform == RuntimePlatform.WebGLPlayer);
#endif
		}

		public static bool IsMouseRightButtonDown()
		{
			if (!EnableInput) return false;
			if ( UtageToolKit.IsPlatformStandAloneOrEditor() || EnableWebGLInput())
			{ 
				return Input.GetMouseButtonDown(1);
			}
			else
			{
				return false;
			}
		}

		public static bool IsInputControl()
		{
			if (!EnableInput) return false;
			if ( UtageToolKit.IsPlatformStandAloneOrEditor() || EnableWebGLInput())
			{ 
				return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			}
			else
			{
				return false;
			}
		}

		static readonly float wheelSensitive = 0.1f;
		public static bool IsInputScrollWheelUp()
		{
			if (!EnableInput) return false;
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis >= wheelSensitive )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsInputScrollWheelDown()
		{
			if (!EnableInput) return false;
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis <= -wheelSensitive )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsInputKeyboadReturnDown()
		{
			if (!EnableInput) return false;
			if (UtageToolKit.IsPlatformStandAloneOrEditor() || EnableWebGLInput())
			{
				return Input.GetKeyDown(KeyCode.Return);
			}
			else
			{
				return false;
			}
		}

		internal static bool GetKeyDown(KeyCode keyCode)
		{
			if (!EnableInput) return false;
			return Input.GetKeyDown(keyCode);
		}
	}

}