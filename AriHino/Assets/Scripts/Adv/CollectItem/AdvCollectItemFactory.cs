using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAdvCollectItemFactory {

    AdvCollectItemGroup CreateCollectItem(int collectItemId);
}

public class AdvCollectItemFactory : MonoBehaviour, IAdvCollectItemFactory
{
    private const string prefabPath = "Adv/CollectItem/CollectItemGroup_{0}";

    public AdvCollectItemGroup CreateCollectItem(int collectItemId){

        var prefab = Resources.Load<AdvCollectItemGroup>(prefabPath);

        if(prefab == null) return null;

        return Instantiate<AdvCollectItemGroup>(prefab);

    }

}
