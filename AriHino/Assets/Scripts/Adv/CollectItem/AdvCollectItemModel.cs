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

    IReadOnlyReactiveDictionary<int, MstCollectItemData> PutCollectItems { get; }

    void SetCollectItem(MstCollectItemData collectItem);
    void RemoveCollectItem(int collectItemId);
}

public class AdvCollectItemModel : IAdvCollectItemModel
{
    // 取得したアイテムリスト
    private ReactiveCollection<MstCollectItemData> hasCollectItems;
    public IList<MstCollectItemData> HasCollectItems => hasCollectItems.ToList();
    public IObservable<CollectionAddEvent<MstCollectItemData>> HasCollectItemAddObservable => hasCollectItems.ObserveAdd();

    // 設置するアイテムリスト
    private ReactiveDictionary<int, MstCollectItemData> putCollectItems = new ReactiveDictionary<int, MstCollectItemData>();
    public IReadOnlyReactiveDictionary<int, MstCollectItemData> PutCollectItems => putCollectItems;

    List<MstCollectItemData> list;

    public AdvCollectItemModel(List<MstCollectItemData> advCollectItemIds) {

        var MstAdvCollectItems = new List<MstCollectItemData>();
        // foreach (var advCollectItemId in advCollectItemIds){
        //     MstAdvCollectItems.Add(new MstAdvCollectItem(advCollectItemId,))
        // }

        hasCollectItems = new ReactiveCollection<MstCollectItemData>(MstAdvCollectItems);
    }

    public void SetCollectItem(MstCollectItemData collectItem){

        putCollectItems.Add(collectItem.ID, collectItem);
    }

    public void SetHasCollectItem(){

    }

    public void RemoveCollectItem(int collectItemId) {

        if(putCollectItems.ContainsKey(collectItemId)) {
            putCollectItems.Remove(collectItemId);
        }
    }

    public void RemoveAllCollectItem() {
        putCollectItems.Clear();
    }
}
