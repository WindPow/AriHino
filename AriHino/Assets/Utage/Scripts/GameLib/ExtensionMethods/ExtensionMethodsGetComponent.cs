// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using UnityEngine.Pool;
using Utage;
using System.Collections.Generic;
using System.Linq;

namespace UtageExtensions
{
	//コンポーネント取得関連の拡張メソッド
	public static class UtageExtensionMethodsGetComponent
	{
		//コンポーネントのキャッシュを取得
		public static T GetComponentCache<T>(this GameObject go, ref T component) where T : class
		{
			if (component == null)
			{
				component = go.GetComponent<T>();
			}
			return component;
		}
		public static T GetComponentCache<T>(this Component target, ref T component) where T : class
		{
			try
			{
				return target.gameObject.GetComponentCache<T>(ref component);
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
				return null;
			}
		}

		//コンポーネントのキャッシュを取得。なかったら作成
		public static T GetComponentCacheCreateIfMissing<T>(this GameObject go, ref T component) where T : Component
		{
			if (component == null)
			{
				component = GetComponentCreateIfMissing<T>(go);
			}
			return component;
		}
		public static T GetComponentCacheCreateIfMissing<T>(this Component target, ref T component) where T : Component
		{
			try
			{
				return target.gameObject.GetComponentCacheCreateIfMissing<T>(ref component);
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
				return null;
			}
		}


		//コンポーネントを取得。なかったら作成
		public static T GetComponentCreateIfMissing<T>(this GameObject go) where T : Component
		{
			T component = go.GetComponent<T>();
			if (component == null)
			{
				component = go.AddComponent<T>();
			}
			return component;
		}
		public static T GetComponentCreateIfMissing<T>(this Component target) where T : Component
		{
			return target.gameObject.GetComponentCreateIfMissing<T>();
		}

		//コンポーネントのキャッシュを子オブジェクトから取得
		public static T GetComponentCacheInChildren<T>(this GameObject go, ref T component) where T : class
		{
			if (component == null)
			{
				component = go.GetComponentInChildren<T>(true);
			}
			return component;
		}


		//コンポーネント配列のキャッシュを子オブジェクトから取得
		public static T[] GetComponentsCacheInChildren<T>(this GameObject go, ref T[] components) where T : class
		{
			if (components == null)
			{
				components = go.GetComponentsInChildren<T>(true);
			}
			return components;
		}
		public static T[] GetComponentsCacheInChildren<T>(this Component target, ref T[] components) where T : class
		{
			try
			{
				return target.gameObject.GetComponentsCacheInChildren<T>(ref components);
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
				return null;
			}
		}

		//コンポーネントのリストのキャッシュを子オブジェクトから取得
		public static List<T> GetComponentListCacheInChildren<T>(this GameObject go, ref List<T> components) where T : class
		{
			if (components == null)
			{
				components = new List<T>(go.GetComponentsInChildren<T>(true));
			}
			return components;
		}
		public static List<T> GetComponentListCacheInChildren<T>(this Component target, ref List<T> components)
			where T : class
		{
			try
			{
				return target.gameObject.GetComponentListCacheInChildren<T>(ref components);
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
				return null;
			}
		}

		//コンポーネントのキャッシュを取得(なかったらシーン内からFind)
		public static T GetComponentCacheFindIfMissing<T>(this GameObject go, ref T component) where T : Component
		{
			if (component == null)
			{
				component = WrapperFindObject.FindObjectOfType<T>();
			}
			return component;
		}
		public static T GetComponentCacheFindIfMissing<T>(this Component target, ref T component) where T : Component
		{
			return target.gameObject.GetComponentCacheFindIfMissing<T>(ref component);
		}


		//コンポーネントのキャッシュを子オブジェクトから取得
		public static T GetComponentCacheInChildren<T>(this Component target, ref T component) where T : Component
		{
			if (component == null)
			{
				component = target.GetComponentInChildren<T>(true);
			}
			return component;
		}

		//コンポーネントのキャッシュを子オブジェクトから取得
		//2個以上あったらエラーメッセージを出す
		public static T GetComponentCacheInChildrenSafe<T>(this GameObject go, ref T component) 
			where T : class
		{
			if (component == null)
			{
				component = go.GetComponentInChildren<T>(true);
#if UNITY_EDITOR
				using (ListPool<T>.Get(out List<T> list) )
				{
					go.GetComponents(list);
					if (list.Count > 2)
					{
						Debug.LogError($"GetComponentCacheInChildrenSafeで取得するコンポーネント{typeof(T)}は二つ以上あります", go );
					}
				}
#endif
			}
			return component;
		}
		public static T GetComponentCacheInChildrenSafe<T>(this Component target, ref T component) 
			where T : class
		{
			return target.gameObject.GetComponentCacheInChildrenSafe(ref component);
		}


		//コンポーネントのキャッシュを取得
		public static T[] GetComponentsCache<T>(this GameObject go, ref T[] components)
			where T: class
		{
			if (components == null)
			{
				components = go.GetComponents<T>();
			}
			return components;
		}
		public static T[] GetComponentsCache<T>(this Component target, ref T[] components) 
			where T: class
		{
			return target.gameObject.GetComponentsCache(ref components);
		}


		//コンポーネントのキャッシュを親オブジェクトから取得
		public static T GetComponentCacheInParent<T>(this GameObject go, ref T component) where T : class
		{
			if (component != null) return component;
			return component = go.GetComponentInParent<T>();
		}
		public static T GetComponentCacheInParent<T>(this Component target, ref T component) where T : class
		{
			if (component != null) return component;
			return component = target.GetComponentInParent<T>();
		}

		//コンポーネントのシングルトンの処理
		public static T GetSingletonFindIfMissing<T>(this T target, ref T instance) where T : Component
		{
			if (instance == null)
			{
				instance = WrapperFindObject.FindObjectOfType<T>();
			}

			return instance;
		}

		//コンポーネントのシングルトンの処理
		public static void InitSingletonComponent<T>(this T target, ref T instance) where T : Component
		{
			if (instance != null && instance != target)
			{
				Debug.LogErrorFormat("{0} is multiple created", typeof(T).ToString());
				GameObject.Destroy(target.gameObject);
			}
			else
			{
				instance = target;
			}
		}



		/// 指定のコンポーネントを階層以下から探し、なかったら子オブジェクトとしてそのコンポーネントを持つ子オブジェクトを作成する
		public static T GetComponentInChildrenCreateIfMissing<T>(this Transform t) where T : Component
		{
			return t.GetComponentInChildrenCreateIfMissing<T>(typeof(T).Name);
		}

		/// 指定のコンポーネントを階層以下から探し、なかったら子オブジェクトとしてそのコンポーネントを持つ子オブジェクトを作成する
		public static T GetComponentInChildrenCreateIfMissing<T>(this Transform t, string name) where T : Component
		{
			T component = t.GetComponentInChildren<T>();
			if (component == null)
			{
				component = t.AddChildGameObjectComponent<T>(name);
			}

			return component;
		}

        
        //子以下のコンポーネントをブロッカー付きで取得
        //ブロッカーに指定した型のコンポーネントを持つものが見つかったら、それ以降の子は検索しない
        public static void GetComponentsWithBlocker<TComponent,TBlocker>(this Transform t, List<TComponent> components, bool includeBlocker = true) 
	        where TComponent : class
	        where TBlocker : class
        {
	        t.gameObject.GetComponentsWithBlocker<TComponent,TBlocker>(components, includeBlocker);
        }
        public static void GetComponentsWithBlocker<TComponent,TBlocker>(this GameObject gameObject, List<TComponent> components, bool includeBlocker = true) 
	        where TComponent : class
	        where TBlocker : class
        {
	        components.Clear();
	        //自身はブロッカーの検知をせずに取得
	        gameObject.GetComponents(components);
	        //子以下はブロッカーの検知して取得
	        foreach (Transform child in gameObject.transform)
	        {
		        child.GetComponentsWithBlockerSub<TComponent, TBlocker>(components,includeBlocker);
	        }
        }

        //子以下のコンポーネントをブロッカー付きで取得
        static void GetComponentsWithBlockerSub<TComponent, TBlocker>(this Transform t,
	        List<TComponent> list, bool includeBlocker )
	        where TComponent : class
	        where TBlocker : class
        {
	        t.gameObject.GetComponentsWithBlockerSub<TComponent, TBlocker>(list, includeBlocker);
        }
        static void GetComponentsWithBlockerSub<TComponent, TBlocker>(this GameObject gameObject,
	        List<TComponent> list, bool includeBlocker )
	        where TComponent : class
	        where TBlocker : class
        {
	        using (ListPool<Component>.Get(out List<Component> components))
	        {
		        gameObject.GetComponents(components);
		        bool isBlocker = components.Exists(x => x is TBlocker or IGetComponentBlockerWild);  
		        if (isBlocker && !includeBlocker)
		        {
			        //ブロッカーがあるのでここまで
			        return;
		        }
		        foreach (var component in components)
		        {
			        if (component is TComponent comp)
			        {
				        list.Add(comp);
			        }
		        }
		        if (isBlocker)
		        {
			        //ブロッカーがあるのでここまで
			        return;
		        }
	        }
	        foreach (Transform child in gameObject.transform)
	        {
		        child.GetComponentsWithBlockerSub<TComponent, TBlocker>(list,includeBlocker);
	        }
        }

        //指定の名前のオブジェクトを子供以下の全ての階層から取得して、そのコンポーネントをGetする
        public static T Find<T>(this Transform t, string name) where T : Component
        {
	        var child = t.Find(name);
	        if (child == null) return null;
	        return child.GetComponent<T>();
        }

        //指定の名前のオブジェクトを子供以下の全ての階層から取得
        public static Transform FindDeep(this Transform t, string name, bool includeInactive = false)
        {
	        var children = t.GetComponentsInChildren<Transform>(includeInactive);
	        foreach (var child in children)
	        {
		        if (child.gameObject.name == name)
		        {
			        return child;
		        }
	        }

	        return null;
        }

        //指定の名前のオブジェクトを子供以下の全ての階層から取得して、そのコンポーネントをGetする
        public static T FindDeepAsComponent<T>(this Transform t, string name, bool includeInactive = false)
	        where T : Component
        {
	        var children = t.GetComponentsInChildren<T>(includeInactive);
	        foreach (var child in children)
	        {
		        if (child.gameObject.name == name)
		        {
			        return child;
		        }
	        }

	        return null;
        }

	}
}
