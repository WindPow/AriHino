// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using UtageExtensions;

namespace Utage
{

	/// <summary>
	/// コマンド：フェードイン処理
	/// </summary>
	internal abstract class AdvCommandFadeBase: AdvCommandEffectBase
		, IAdvCommandEffect
	{
		float time;
		bool inverse;
		Color color;
		string ruleImage;
		float vague;
		Timer Timer { get; set; }

		public AdvCommandFadeBase(StringGridRow row, bool inverse)
			: base(row)
		{
			this.inverse = inverse;
		}

		protected override void OnParse()
		{
			this.color = ParseCellOptional<Color>(AdvColumnName.Arg1, Color.white);
			if (IsEmptyCell(AdvColumnName.Arg2))
			{
				this.targetName = "SpriteCamera";
			}
			else
			{
				//第2引数はターゲットの設定
				this.targetName = ParseCell<string>(AdvColumnName.Arg2);
			}

			this.time = ParseCellOptional<float>(AdvColumnName.Arg6,0.2f);
			this.ruleImage = ParseCellOptional(AdvColumnName.Arg3, "");
			this.vague = ParseCellOptional(AdvColumnName.Arg4, 0.2f);

			this.targetType = AdvEffectManager.TargetType.Camera;

			ParseWait(AdvColumnName.WaitType);
		}

		protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
		{
			Camera camera = target.GetComponentInChildren<Camera>(true);

			float start, end;
			ImageEffectBase imageEffect = null;
			IImageEffectStrength effectStrength = null;
			if (string.IsNullOrEmpty(ruleImage))
			{
				bool alreadyEnabled;
				bool ruleEnabled = camera.gameObject.GetComponent<RuleFade>();
				if (ruleEnabled)
				{
					camera.gameObject.SafeRemoveComponent<RuleFade>();
				}
				ImageEffectUtil.TryGetComonentCreateIfMissing(ImageEffectType.ColorFade.ToString(), out imageEffect, out alreadyEnabled, camera.gameObject);
				effectStrength = imageEffect as IImageEffectStrength;
				ColorFade colorFade = imageEffect as ColorFade;
				if (inverse)
				{
					//画面全体のフェードイン（つまりカメラのカラーフェードアウト）
					//					start = colorFade.color.a;
					start = (ruleEnabled) ? 1 : colorFade.color.a;
					end = 0;
				}
				else
				{
					//画面全体のフェードアウト（つまりカメラのカラーフェードイン）
					//colorFade.Strengthで、すでにフェードされているのでそちらの値をつかう
					start = alreadyEnabled ? colorFade.Strength : 0;
					end = this.color.a;
				}
				colorFade.enabled = true;
				colorFade.color = color;
			}
			else
			{
				bool alreadyEnabled;
				camera.gameObject.SafeRemoveComponent<ColorFade>();
				ImageEffectUtil.TryGetComonentCreateIfMissing(ImageEffectType.RuleFade.ToString(), out imageEffect, out alreadyEnabled, camera.gameObject);
				effectStrength = imageEffect as IImageEffectStrength;
				RuleFade ruleFade = imageEffect as RuleFade;
				ruleFade.ruleTexture = engine.EffectManager.FindRuleTexture(ruleImage);
				ruleFade.vague = vague;
				if (inverse)
				{
					start = 1;
					end = 0;
				}
				else
				{
					start = alreadyEnabled ? ruleFade.Strength : 0;
					end = 1;
				}
				ruleFade.enabled = true;
				ruleFade.color = color;
			}

			Timer = camera.gameObject.AddComponent<Timer>();
			Timer.AutoDestroy = true;
			Timer.StartTimer(
				engine.Page.ToSkippedTime(this.time),
				engine.Time.Unscaled,
				(x) =>
				{
					effectStrength.Strength = x.GetCurve(start, end);
				},
				(x) =>
				{
					OnComplete(thread);
					if (inverse)
					{
						imageEffect.enabled = false;
						imageEffect.RemoveComponentMySelf();
					}
				});
		}
		
		public void OnEffectSkip()
		{
			if (Timer == null) return;
			Timer.SkipToEnd();
		}

		public void OnEffectFinalize()
		{
			Timer = null;
		}
	}
}
