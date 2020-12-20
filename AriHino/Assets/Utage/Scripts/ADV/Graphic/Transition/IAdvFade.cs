// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using System;

namespace Utage
{
	// フェード処理用のインターフェース
	public interface IAdvFade
	{
		//フェードイン
		void FadeIn(float time, Action onComplete);

		//フェードアウト
		void FadeOut(float time, Action onComplete);

		//ルール画像つきのフェードイン
		void RuleFadeIn(AdvEngine engine, AdvTransitionArgs data, Action onComplete);

		//ルール画像つきのフェードアウト
		void RuleFadeOut(AdvEngine engine, AdvTransitionArgs data, Action onComplete);
	}
	
	
	// スキップ可能なフェード処理用のインターフェース
	public interface IAdvFadeSkippable : IAdvFade
	{
		//フェードアウト
//		void FadeInOrCrossFadeRestart(float time, Action onComplete);
		
		//フェードをスキップする
//		void SkipFade();

		//ルール画像付きのフェードをスキップする
		void SkipRuleFade();
		
		//削除
//		void Clear();
	}
}