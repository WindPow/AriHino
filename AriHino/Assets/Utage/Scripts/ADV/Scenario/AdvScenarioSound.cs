// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UtageExtensions;

namespace Utage
{
	//シナリオ中に再生されているサウンドの管理
	//主にシナリオ中のボイス再生と、バックログからのボイス再生かの区別をつけるために使う
	[AddComponentMenu("Utage/ADV/Internal/AdvScenarioSound")]
	public class AdvScenarioSound : MonoBehaviour
	{
		//現在のシナリオ中のボイスかどうかを区別しない場合（旧仕様のまま）はtrueに
		bool DisableScenarioVoce => disableScenarioVoce; 
		[SerializeField] bool disableScenarioVoce = false;
		
		Dictionary<string, AssetFile> VoiceFilesInScenario { get; } = new Dictionary<string, AssetFile>();

		AdvEngine Engine => this.GetComponentCacheInParent(ref engine);
		AdvEngine engine;
		SoundManager SoundManager => Engine.SoundManager;

		public void Clear()
		{
			VoiceFilesInScenario.Clear();
		}
		//現在のシナリオ中のボイスを再生する時に呼ぶ
		public void SetVoiceInScenario(string characterLabel, AssetFile file )
		{
			if (VoiceFilesInScenario.ContainsKey(characterLabel))
			{
				VoiceFilesInScenario[characterLabel] = file;
			}
			else
			{
				VoiceFilesInScenario.Add(characterLabel,file);
			}
		}
		
		//現在のシナリオ外のボイスを再生する時に呼ぶ
		//バックログなど、シナリオ中のボイスと同じファイルが再生される可能性がある場合、区別をつけるために呼ぶ
		public void ClearVoiceInScenario(string characterLabel)
		{
			if (VoiceFilesInScenario.ContainsKey(characterLabel))
			{
				VoiceFilesInScenario[characterLabel] = null;
			}
		}

		//リップシンク対象となるボイスが鳴っているか
		public bool CheckLipSync(string characterLabel)
		{
			if (characterLabel.IsNullOrEmpty()) return false;
			if (SoundManager==null) return false;
			if (!IsPlayingScenarioVoice(characterLabel)) return false;

			var samplesVolume = SoundManager.GetVoiceSamplesVolume(characterLabel);
//			Debug.Log($"samplesVolume={samplesVolume}");
			return samplesVolume > 0.3f;
		}

		//現在のシナリオ内のボイスが再生されているか
		public bool IsPlayingScenarioVoice(string characterLabel)
		{
			if (characterLabel.IsNullOrEmpty()) return false;
			if (SoundManager==null) return false;
			
			if (DisableScenarioVoce)
			{
				//現在のシナリオ中のボイスかどうかを区別しない場合（旧仕様のまま）
				return SoundManager.IsPlayingVoice(characterLabel);
			}
			else
			{
				if (!VoiceFilesInScenario.TryGetValue(characterLabel, out AssetFile file))
				{
					return false;
				}

				if (file == null)
				{
					return false;
				}

				var soundManagerSystemEx = SoundManager.SystemEx; 
				if (soundManagerSystemEx != null)
				{
					return soundManagerSystemEx.IsPlaying(SoundManager.IdVoice,characterLabel,file);
				}
				else
				{
					return SoundManager.IsPlayingVoice(characterLabel);
				}
			}
		}
	}
}
