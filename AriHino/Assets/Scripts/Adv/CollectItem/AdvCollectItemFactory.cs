using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

/// <summary>
/// DataViewを生成して返す
/// </summary>
public interface IAdvCollectItemFactory {
    IAdvCollectItemDataView CreateCollectItem(int collectItemId);
}

public class AdvCollectItemFactory : MonoBehaviour, IAdvCollectItemFactory
{
    [SerializeField] Transform parent;

    private const string prefabPath = "Prefabs/Adv/CollectItem/CollectItem_{0}";

    public IAdvCollectItemDataView CreateCollectItem(int collectItemId){

        var prefab = Resources.Load<AdvCollectItemDataView>(ZString.Format(prefabPath, collectItemId));

        if(prefab == null) return null;

        return Instantiate(prefab, parent);

    }

}
