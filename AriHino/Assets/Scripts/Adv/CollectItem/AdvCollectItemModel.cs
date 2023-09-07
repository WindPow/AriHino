using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UniRx;
using System.Linq;
using System;

/// <summary>
/// コレクトアイテムモデル
/// </summary>
interface IAdvCollectItemModel {

    IList<MstAdvCollectItem> HasCollectItems { get; }
    IObservable<CollectionAddEvent<MstAdvCollectItem>> HasCollectItemAddObservable { get; }

    public IList<MstAdvCollectItem> PutCollectItems { get; }
    public IObservable<CollectionAddEvent<MstAdvCollectItem>> PutCollectItemAddObservable { get; }
    public IObservable<CollectionRemoveEvent<MstAdvCollectItem>> PutCollectItemRemoveObservable { get;}

    void SetCollectItem(MstAdvCollectItem collectItem);
}

public class AdvCollectItemModel : IAdvCollectItemModel
{
    // 取得したアイテムリスト
    private ReactiveCollection<MstAdvCollectItem> hasCollectItems;
    public IList<MstAdvCollectItem> HasCollectItems => hasCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstAdvCollectItem>> HasCollectItemAddObservable => hasCollectItems.ObserveAdd();

    // 設置するアイテムリスト
    private ReactiveCollection<MstAdvCollectItem> putCollectItems = new ReactiveCollection<MstAdvCollectItem>();
    public IList<MstAdvCollectItem> PutCollectItems => putCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstAdvCollectItem>> PutCollectItemAddObservable => putCollectItems.ObserveAdd();
    public IObservable<CollectionRemoveEvent<MstAdvCollectItem>> PutCollectItemRemoveObservable => putCollectItems.ObserveRemove();

    public AdvCollectItemModel(List<MstAdvCollectItem> advCollectItemIds) {

        var MstAdvCollectItems = new List<MstAdvCollectItem>();
        foreach (var advCollectItemId in advCollectItemIds){
            //MstAdvCollectItems.Add(new MstAdvCollectItem(advCollectItemId,))
        }

        hasCollectItems = new ReactiveCollection<MstAdvCollectItem>(MstAdvCollectItems);
    }

    public void SetCollectItem(MstAdvCollectItem collectItem){

        putCollectItems.Add(collectItem);
    }

    public void SetHasCollectItem(){

    }
}
