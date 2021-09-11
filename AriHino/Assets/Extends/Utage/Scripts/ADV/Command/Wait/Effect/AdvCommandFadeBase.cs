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
		AdvAnimationData animationData;
		protected AdvAnimationPlayer AnimationPlayer { get; set; }

		protected AdvCommandFadeBase(StringGridRow row, AdvSettingDataManager dataManager, bool inverse)
			: base(row, dataManager)
		{
			this.inverse = inverse;
		}

		protected override void OnParse(AdvSettingDataManager dataManager)
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

			this.ruleImage = ParseCellOptional(AdvColumnName.Arg3, "");
			this.vague = ParseCellOptional(AdvColumnName.Arg4, 0.2f);
			this.targetType = AdvEffectManager.TargetType.Camera;
			string arg6 = ParseCellOptional<string>(AdvColumnName.Arg6,"");
			this.animationData = null;
			this.time = 0.2f;
			if (!arg6.IsNullOrEmpty())
			{
				float f;
				if (WrapperUnityVersion.TryParseFloatGlobal(arg6, out f))
				{
					time = f;
				}
				else
				{
					animationData = dataManager.AnimationSetting.Find(arg6);
					if (animationData == null)
					{
						Debug.LogError(RowData.ToErrorString("Animation " + arg6 + " is not found"));
					}
				}
			}

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

			if (animationData==null)
			{
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
			else
			{
				//アニメーションを再生
				AnimationPlayer = imageEffect.gameObject.AddComponent<AdvAnimationPlayer>();
				AnimationPlayer.AutoDestory = true;
				AnimationPlayer.Play(animationData.Clip, engine.Page.SkippedSpeed,
					() =>
					{
						OnComplete(thread);
					});

			}
		}
		
		public void OnEffectSkip()
		{
			if (Timer != null)
			{
				Timer.SkipToEnd();
			}
			if (AnimationPlayer != null)
			{
				AnimationPlayer.SkipToEnd();
			}
		}

		public void OnEffectFinalize()
		{
			Timer = null;
			AnimationPlayer = null;
		}
	}
}
