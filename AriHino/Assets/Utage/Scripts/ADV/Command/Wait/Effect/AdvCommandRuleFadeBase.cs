// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using UtageExtensions;

namespace Utage
{

	internal abstract class AdvCommandRuleFadeBase : AdvCommandEffectBase
		, IAdvCommandEffect
	{
		protected IAdvFadeSkippable Fade { get; set; }
		protected AdvTransitionArgs TransitionArgs { get; set; }
		protected AdvCommandRuleFadeBase(StringGridRow row)
			: base(row)
		{
			string textureName = ParseCell<string>(AdvColumnName.Arg2);
			float vague = ParseCellOptional<float>(AdvColumnName.Arg3, 0.2f);
			float time = ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
			this.TransitionArgs = new AdvTransitionArgs(textureName, vague, time);
		}

		//エフェクト開始時のコールバック
		protected override void OnStartEffect( GameObject target, AdvEngine engine, AdvScenarioThread thread)
		{
			this.Fade = target.GetComponentInChildren<IAdvFadeSkippable>(true);
			if (Fade == null)
			{
				Debug.LogError("Can't find [ " + this.TargetName +" ]");
				OnComplete(thread);
			}
			else
			{
				OnStartFade(target, engine, thread);
			}
		}

		//フェード開始時のコールバック
		protected abstract void OnStartFade(GameObject target, AdvEngine engine, AdvScenarioThread thread);

		//エフェクト開始時のコールバック
		public void OnEffectSkip()
		{
			if (Fade == null)
			{
				return;
			}
			OnSkipFade();
		}

		//フェードスキップ時
		protected virtual void OnSkipFade()
		{
			Fade.SkipRuleFade();
		}

		//エフェクト終了時
		public void OnEffectFinalize()
		{
			Fade = null;
		}
	}
}