using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataViewを生成して返す
/// </summary>
public interface IAdvCollectItemFactory {
    IAdvCollectItemDataView CreateCollectItem(int collectItemId);
}

public class AdvCollectItemFactory : MonoBehaviour, IAdvCollectItemFactory
{
    private const string prefabPath = "Adv/CollectItem/CollectItemGroup_{0}";

    public IAdvCollectItemDataView CreateCollectItem(int collectItemId){

        var prefab = Resources.Load<AdvCollectItemDataView>(prefabPath);

        if(prefab == null) return null;

        return Instantiate<AdvCollectItemDataView>(prefab);

    }

}
