// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace Utage
{
	//シナリオスレッド内のコマンド待機処理のマネージャー
	internal class AdvWaitManager
	{
		//管理しているコマンドリスト
		readonly List<AdvCommandWaitBase> notWaitCommandList = new List<AdvCommandWaitBase>();
		readonly List<AdvCommandWaitBase> waitCommandList = new List<AdvCommandWaitBase>();
		
		//CheckWaitで削除対象となるコマンドリスト
		//Alloc回避にリストのインスタンスは保持する
		readonly List<AdvCommandWaitBase> removeCommandList = new List<AdvCommandWaitBase>();

		internal void Clear()
		{
			ClearCommandList(notWaitCommandList);
			ClearCommandList(waitCommandList);
		}

		void ClearCommandList(List<AdvCommandWaitBase> list)
		{
			foreach (var item in list)
			{
				FinalizeCommand(item);
			}
			list.Clear();
		}

		void FinalizeCommand(AdvCommandWaitBase command)
		{
			var effect = command as IAdvCommandEffect;
			if (effect != null)
			{
				effect.OnEffectFinalize();
			}
		}

		internal void StartCommand(AdvCommandWaitBase command)
		{
			//タイプによって管理リストから除外
			switch (command.WaitType)
			{
				case AdvCommandWaitType.NoWait:
					notWaitCommandList.Add(command);
					break;
				default:
					waitCommandList.Add(command);
					break;
			}
		}

		internal void CompleteCommand(AdvCommandWaitBase command)
		{
			FinalizeCommand(command);
			//タイプによって管理リストから除外
			switch (command.WaitType)
			{
				case AdvCommandWaitType.NoWait:
					notWaitCommandList.Remove(command);
					break;
				default:
					waitCommandList.Remove(command);
					break;
			}
		}

		//何らかの待機あり
		internal bool IsWaiting
		{
			get { return waitCommandList.Count > 0; }
		}


		//待機コマンドの場合のチェック
		internal bool IsWaitingDefault
		{
			get
			{
				UpdateCheckWait();
				return waitCommandList.Exists(x => x.WaitType.IsWaitingCommandType());
			}
		}

		//改行入力などを入力前にするエフェクトの終了待ち
		internal bool IsWaitingInputEffect
		{
			get
			{
				UpdateCheckWait();
				return waitCommandList.Exists(x => x.WaitType.IsWaitingInputType());
			}
		}

		//改ページ入力前にするエフェクトの終了待ち
		internal bool IsWaitingPageEndEffect
		{
			get
			{
				UpdateCheckWait();
				return waitCommandList.Exists(x => x.WaitType.IsWaitingPageEndEffect());
			}
		}

		internal bool IsWaitingOnThread
		{
			get
			{
				UpdateCheckWait();
				return waitCommandList.Exists(x => x.WaitType.IsWaitingOnThreadType());
			}
		}

		//コールバックでは実行されず、終わったかの終了チェックが必要なものをここで呼ぶ
		//対象のコマンドがWait待機中にしか呼ばれないので、毎フレームの時間加算などには使えない点に注意
		void UpdateCheckWait()
		{
			removeCommandList.Clear();
			foreach ( var command in waitCommandList)
			{
				IAdvCommandUpdateWait checkWait = command as IAdvCommandUpdateWait;
				if (checkWait != null )
				{
					if (!checkWait.UpdateCheckWait())
					{
						removeCommandList.Add(command);
					}
				}
			}
			if(removeCommandList.Count>0)
			{
				foreach (var command in removeCommandList)
				{
					CompleteCommand(command);
				}
				removeCommandList.Clear();
			}
		}

		//コマンド待ちでのスキップ
		public void SkipEffectCommand()
		{
			SkipEffectSub(x => x.IsSkippableCommand());
		}

		//入力待ちでのスキップ
		public void SkipEffectInput()
		{
			SkipEffectSub(x => x.IsSkippableInput());
		}

		//改ページ待ちでのスキップ
		public void SkipEffectPageEnd()
		{
			SkipEffectSub(x => x.IsSkippable());
		}

		//WaitThreadコマンド中の演出スキップ
		public void SkipEffectCommandOnWaitThread()
		{
			SkipEffectSub(x => x.IsSkippableCommandOnWaitThread());
		}

		void SkipEffectSub( Func<AdvCommandWaitType,bool> checkSkip )
		{
			if (!waitCommandList.Exists(x=>checkSkip(x.WaitType)))
			{
				return;
			}

			var list = new List<AdvCommandWaitBase>(waitCommandList);
			foreach (AdvCommandWaitBase command in list)
			{
				if (!checkSkip(command.WaitType))
				{
//					Debug.LogWarning( command.ToErrorString("Not Skippable Wait type" + command.WaitType +" is added"));
					continue;
				}

				IAdvCommandEffect skip = command as IAdvCommandEffect;
				if (skip != null)
				{
					skip.OnEffectSkip();
				}
				else
				{
					Debug.LogErrorFormat("command {0} is not skippable effect", command.Id);
				}
			}
		}
	}
}