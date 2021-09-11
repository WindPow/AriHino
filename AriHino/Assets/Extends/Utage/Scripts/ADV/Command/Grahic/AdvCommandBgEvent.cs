// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：背景表示・切り替え
	/// </summary>
	internal class AdvCommandBgEvent : AdvCommandBgBase
	{
		public AdvCommandBgEvent(StringGridRow row, AdvSettingDataManager dataManager)
			: base(row, dataManager)
		{
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.SystemSaveData.GalleryData.AddCgLabel(label);
			engine.GraphicManager.IsEventMode = true;
			//表示する
			AdvGraphicOperationArg graphicOperationArg = DoCommandBgSub(engine);
			//キャラクターは非表示にする
			engine.GraphicManager.CharacterManager.FadeOutAll(graphicOperationArg.GetSkippedFadeTime(engine));
		}
	}
}