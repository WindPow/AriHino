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

    IList<MstCollectItemData> HasCollectItems { get; }
    IObservable<CollectionAddEvent<MstCollectItemData>> HasCollectItemAddObservable { get; }

    IList<MstCollectItemData> PutCollectItems { get; }
    IObservable<CollectionAddEvent<MstCollectItemData>> PutCollectItemAddObservable { get; }
    IObservable<CollectionRemoveEvent<MstCollectItemData>> PutCollectItemRemoveObservable { get;}

    void SetCollectItem(MstCollectItemData collectItem);
}

public class AdvCollectItemModel : IAdvCollectItemModel
{
    // 取得したアイテムリスト
    private ReactiveCollection<MstCollectItemData> hasCollectItems;
    public IList<MstCollectItemData> HasCollectItems => hasCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstCollectItemData>> HasCollectItemAddObservable => hasCollectItems.ObserveAdd();

    // 設置するアイテムリスト
    private ReactiveCollection<MstCollectItemData> putCollectItems = new ReactiveCollection<MstCollectItemData>();
    public IList<MstCollectItemData> PutCollectItems => putCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstCollectItemData>> PutCollectItemAddObservable => putCollectItems.ObserveAdd();
    public IObservable<CollectionRemoveEvent<MstCollectItemData>> PutCollectItemRemoveObservable => putCollectItems.ObserveRemove();

    List<MstCollectItemData> list;

    public AdvCollectItemModel(List<MstCollectItemData> advCollectItemIds) {

        var MstAdvCollectItems = new List<MstCollectItemData>();
        // foreach (var advCollectItemId in advCollectItemIds){
        //     MstAdvCollectItems.Add(new MstAdvCollectItem(advCollectItemId,))
        // }

        hasCollectItems = new ReactiveCollection<MstCollectItemData>(MstAdvCollectItems);
    }

    public void SetCollectItem(MstCollectItemData collectItem){

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
