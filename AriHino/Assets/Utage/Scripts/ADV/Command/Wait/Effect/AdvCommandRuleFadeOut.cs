// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using UtageExtensions;

namespace Utage
{

	// コマンド：ルール画像付きのフェードアウト
	internal class AdvCommandRuleFadeOut : AdvCommandRuleFadeBase
	{
		public AdvCommandRuleFadeOut(StringGridRow row)
			: base(row)
		{
		}

		//フェード開始
		protected override void OnStartFade(GameObject target, AdvEngine engine, AdvScenarioThread thread)
		{
			Fade.RuleFadeOut(engine, this.TransitionArgs, ()=>OnComplete(thread));
		}
	}
}
