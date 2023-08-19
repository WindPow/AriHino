using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UniRx;

interface IAdvCollectItemModel {

    IReadOnlyReactiveProperty<int> CollectItemGroupId { get;}
}

public class AdvCollectItemModel : IAdvCollectItemModel
{

    public IReadOnlyReactiveProperty<int> CollectItemGroupId => collectItemGroupId;
    private ReactiveProperty<int> collectItemGroupId = new ReactiveProperty<int>();

    public void SetCollectItem(int collectItemId){

        collectItemGroupId.Value = collectItemId;
    }
}
