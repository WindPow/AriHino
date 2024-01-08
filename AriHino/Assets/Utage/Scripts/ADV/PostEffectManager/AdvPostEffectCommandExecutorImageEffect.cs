using System;
using UnityEngine;
using UtageExtensions;

namespace Utage
{
	//ポストエフェクトとしてイメージエフェクトコマンドを実行するコンポーネント
	public class AdvPostEffectCommandExecutorImageEffect : AdvPostEffectCommandExecutorBase
	{
/*		
		public void DoCommand( Camera targetCamera, IAdvCommandImageEffect command,  Action onComplete)
		{
			(IPostEffect effect, float start, float end) = RpBridge.DoCommandImageEffect(targetCamera, command);
			
			if (command.AnimationData==null && effect is IPostEffectStrength effectStrength )
			{
				command.Timer = SetTimer(effect, command.Time,
						(x) => effectStrength.Strength = x.GetCurve(start, end),
						(x) =>
						{
							onComplete();
							if (command.Inverse)
							{
								effect.enabled = false;
//								effect.RemoveComponentMySelf();
							}
						});
			}
			else
			{
				command.AnimationPlayer = SetAnimation(effect, command.AnimationData, onComplete);
			}
		}
*/		
		public void DoCommand( Camera targetCamera, IAdvCommandImageEffect command,  Action onComplete)
		{
			(IPostEffect effect, Action complete) = RpBridge.DoCommandImageEffect(targetCamera, command,  onComplete);
			bool enableAnimation = command.AnimationData != null;
			IPostEffectStrength effectStrength = effect as IPostEffectStrength;

			if (effectStrength==null && !enableAnimation)
			{
				complete();
				return;
			}

			if (effectStrength!=null)
			{
				float start = command.Inverse ? effectStrength.Strength : 0;
				float end = command.Inverse ? 0 : 1;
				command.Timer = SetTimer(effect, command.Time,
					(x) => effectStrength.Strength = x.GetCurve(start, end),
					(x) =>
					{
						if (!enableAnimation)
						{
							complete();
						}
					});
			}

			if(enableAnimation)
			{
				command.AnimationPlayer = SetAnimation(effect, command.AnimationData,
					() =>
					{
						complete();
					});
			}
		}
		

		public void DoCommandAllOff(Camera targetCamera, IAdvCommandImageEffect command, Action onComplete)
		{
			RpBridge.DoCommandImageEffectAllOff(targetCamera, command,  onComplete);
		}
	}
}
