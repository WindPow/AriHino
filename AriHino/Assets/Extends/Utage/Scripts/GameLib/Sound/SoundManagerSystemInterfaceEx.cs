// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Utage
{
	// サウンド管理のインターフェース
	public interface SoundManagerSystemInterfaceEx
	{
		// 指定のグループのサウンドが鳴っているか
		bool IsPlaying(string groupName);
	}
}
