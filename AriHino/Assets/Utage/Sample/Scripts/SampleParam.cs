// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using Utage;
using UtageExtensions;
using System.Collections;


/// <summary>
/// Sample LoadErrorのコールバック関数を書き換え
/// </summary>
[AddComponentMenu("Utage/ADV/Examples/SampleParam")]
public class SampleParam : MonoBehaviour
{
	/// <summary>ADVエンジン</summary>
	public AdvEngine Engine { get { return this.GetComponentCacheFindIfMissing( ref engine ); } }
	[SerializeField]
	protected AdvEngine engine;

	public void ParamTest()
	{
		Engine.Param.GetParameter("flag1");
		Engine.Param.TrySetParameter("flag1",true);
	}
}
