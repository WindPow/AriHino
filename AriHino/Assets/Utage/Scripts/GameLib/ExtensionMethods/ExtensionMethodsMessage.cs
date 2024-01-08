#if false
// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Profiling;
using Utage;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UtageExtensions
{
	//指定の型のコンポーネントのメッソドを呼ぶための拡張メソッド
	//特定の型のコンポーネント以外に、インターフェースも対応可能にすることで、メッセージシステム的な動きを可能にする
	//型の取得をListPool<T>で行い、GCの発生を抑えている
	//ただしListPool<T>の性質上、型の種類がふえるとstaticな領域のメモリをListで消費していく点には注意
	public static class UtageExtensionMethodsMessage
	{
		//自身と同じGameObjectが持つ指定の型のコンポーネントを持つものをすべて取得し、指定したコールバックに返す
		public static void ForeachInterface<T>(this GameObject target, System.Action<T> callback)
			where T : class
		{
			UnityEngine.Profiling.Profiler.BeginSample("ForeachInterface");
			using (ListPool<T>.Get(out List<T> list))
			{
				target.GetComponents(list);
				foreach (var item in list)
				{
					callback(item);
				}
			}

			UnityEngine.Profiling.Profiler.EndSample();
		}
		public static void ForeachInterface<T>(this Component target, System.Action<T> callback)
			where T : class
		{
			target.gameObject.ForeachInterface(callback);
		}

		//自身と同じGameObjectが持つ指定の型のコンポーネントを持つものをすべて取得し、指定したコールバックに返す
		//変数キャプチャ対策のための引数設定付き
		public static void ForeachInterfaceNoCapture<T, TArg>(this GameObject target, TArg arg,
			System.Action<T, TArg> callback)
			where T : class
		//			where TArg : class	//IL2CPP対策にジェネリックに値型は排除したいところだが、使用したいケースもあるのであえて許容する	
		{
			UnityEngine.Profiling.Profiler.BeginSample("ForeachInterfaceNoCapture");
			using (ListPool<T>.Get(out List<T> list))
			{
				target.GetComponents(list);
				foreach (var item in list)
				{
					callback(item, arg);
				}
			}

			UnityEngine.Profiling.Profiler.EndSample();
		}
		public static void ForeachInterfaceNoCapture<T, TArg>(this Component target, TArg arg,
			System.Action<T, TArg> callback)
			where T : class
		{
			target.gameObject.ForeachInterfaceNoCapture(arg, callback);
		}


		//自身と子以下のGameObjectが持つ指定の型のコンポーネントを持つものをすべて取得し、指定したコールバックに返す
		public static void ForeachInChildren<T>(this GameObject target, System.Action<T> callback, bool includeInactive = true)
			where T: class
		{
			Profiler.BeginSample("ForeachInterfaceInChildren");
			using (UnityEngine.Pool.ListPool<T>.Get(out List<T> list))
			{
				target.GetComponentsInChildren(includeInactive, list);
				foreach (var item in list)
				{
					callback(item);
				}
			}
			Profiler.EndSample();
		}
		public static void ForeachInChildren<T>(this Component target, System.Action<T> callback, bool includeInactive = true)
			where T: class
		{
			target.gameObject.ForeachInChildren(callback,includeInactive);
		}

		//自身と子以下のGameObjectが持つ指定の型のコンポーネントを持つものをすべて取得し、指定したコールバックに返す
		//変数キャプチャ対策のための引数設定付き
		public static void ForeachInChildrenNoCapture<T, TArg>(this GameObject target, TArg arg, System.Action<T, TArg> callback)
			where T: class
			where TArg : class
		{
			Profiler.BeginSample("ForeachInterfaceInChildrenNoCapture");
			using (ListPool<T>.Get(out List<T> list))
			{
				target.GetComponentsInChildren(true, list);
				foreach (var item in list)
				{
					callback(item, arg);
				}
			}
			Profiler.EndSample();
		}
		public static void ForeachInChildrenNoCapture<T, TArg>(this Component target, TArg arg, System.Action<T, TArg> callback)
			where T: class
			where TArg : class
		{
			target.gameObject.ForeachInChildrenNoCapture(arg,callback);
		}
        
        

        //自身と子以下のコンポーネントから、指定の型を取得してコールバックとして返す
        //指定のBlockerがあった場合そこで止める
        public static void ForeachInChildrenWithBlocker<TComponent,TBlocker>(this Component target, System.Action<TComponent> callback, bool includeBlocker = true)
	        where TComponent : class
	        where TBlocker : class
        {
	        target.gameObject.ForeachInChildrenWithBlocker<TComponent, TBlocker>(callback, includeBlocker);
        }
        public static void ForeachInChildrenWithBlocker<TComponent,TBlocker>(this GameObject gameObject, System.Action<TComponent> callback, bool includeBlocker = true)
	        where TComponent : class
	        where TBlocker : class
        {
	        using (ListPool<TComponent>.Get(out List<TComponent> list))
	        {
		        gameObject.transform.GetComponentsWithBlocker<TComponent,TBlocker>(list,includeBlocker);
		        foreach (var item in list)
		        {
			        callback(item);
		        }
	        }
        }

        //自身と子以下のコンポーネントから、指定の型を取得してコールバックとして返す
        //指定のBlockerがあった場合そこで止める
        public static void ForeachInChildrenWithBlockerNoCapture<TComponent,TBlocker,TArg>(this Component target, TArg arg, System.Action<TComponent,TArg> callback, bool includeBlocker = true)
	        where TComponent : class
	        where TBlocker : class
	        where TArg : class
        {
	        target.gameObject.ForeachInChildrenWithBlockerNoCapture<TComponent, TBlocker,TArg>(arg,callback, includeBlocker);
        }
        public static void ForeachInChildrenWithBlockerNoCapture<TComponent,TBlocker,TArg>(this GameObject gameObject, TArg arg, System.Action<TComponent,TArg> callback, bool includeBlocker = true)
	        where TComponent : class
	        where TBlocker : class
	        where TArg : class
        {
	        using (ListPool<TComponent>.Get(out List<TComponent> list))
	        {
		        gameObject.transform.GetComponentsWithBlocker<TComponent,TBlocker>(list,includeBlocker);
		        foreach (var item in list)
		        {
			        callback(item,arg);
		        }
	        }
        }
        
        //自身と親以上にある指定の型のコンポーネントを持つものをすべて取得し、指定したコールバックに返す
        public static void ForeachInParent<T>(this GameObject target, System.Action<T> callback)
	        where T : class
        {
	        Profiler.BeginSample("ForeachInParent");
	        using (ListPool<T>.Get(out List<T> list))
	        {
		        target.GetComponentsInParent<T>(true, list);
		        foreach (var item in list)
		        {
			        callback(item);
		        }
	        }

	        Profiler.EndSample();
        }
	}
}
#endif
