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

    IList<MstAdvCollectItemData> HasCollectItems { get; }
    IObservable<CollectionAddEvent<MstAdvCollectItemData>> HasCollectItemAddObservable { get; }

    IList<MstAdvCollectItemData> PutCollectItems { get; }
    IObservable<CollectionAddEvent<MstAdvCollectItemData>> PutCollectItemAddObservable { get; }
    IObservable<CollectionRemoveEvent<MstAdvCollectItemData>> PutCollectItemRemoveObservable { get;}

    void SetCollectItem(MstAdvCollectItemData collectItem);
}

public class AdvCollectItemModel : IAdvCollectItemModel
{
    // 取得したアイテムリスト
    private ReactiveCollection<MstAdvCollectItemData> hasCollectItems;
    public IList<MstAdvCollectItemData> HasCollectItems => hasCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstAdvCollectItemData>> HasCollectItemAddObservable => hasCollectItems.ObserveAdd();

    // 設置するアイテムリスト
    private ReactiveCollection<MstAdvCollectItemData> putCollectItems = new ReactiveCollection<MstAdvCollectItemData>();
    public IList<MstAdvCollectItemData> PutCollectItems => putCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstAdvCollectItemData>> PutCollectItemAddObservable => putCollectItems.ObserveAdd();
    public IObservable<CollectionRemoveEvent<MstAdvCollectItemData>> PutCollectItemRemoveObservable => putCollectItems.ObserveRemove();

    List<MstAdvCollectItemData> list;

    public AdvCollectItemModel(List<MstAdvCollectItemData> advCollectItemIds) {

        var MstAdvCollectItems = new List<MstAdvCollectItemData>();
        // foreach (var advCollectItemId in advCollectItemIds){
        //     MstAdvCollectItems.Add(new MstAdvCollectItem(advCollectItemId,))
        // }

        hasCollectItems = new ReactiveCollection<MstAdvCollectItemData>(MstAdvCollectItems);
    }

    public void SetCollectItem(MstAdvCollectItemData collectItem){

        putCollectItems.Add(collectItem);
    }

    public void SetHasCollectItem(){

    }

    public void RemoveCollectItem(int collectItemId) {

        var removeItem = putCollectItems.FirstOrDefault(e => e.ID == collectItemId);
        if (removeItem != null) putCollectItems.Remove(removeItem);
    }

    public void RemoveAllCollectItem() {
        putCollectItems.Clear();
    }
}
