using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// コレクトアイテムプレゼンター
/// </summary>
public class AdvCollectItemPresenter : MonoBehaviour
{
    [SerializeField] private AdvCollectItemFactory advCollectItemFactory;

    [SerializeField] private AdvCollectNotification advCollectNotification;
    
    private IAdvCollectItemModel collectItemModel;

    private Dictionary<int, IAdvCollectItemDataView> displayCollectItems = new Dictionary<int, IAdvCollectItemDataView>();

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="collectItemModel"></param>
    public void Init(AdvCollectItemModel collectItemModel){

        this.collectItemModel = collectItemModel;

        Bind();

    }

    /// <summary>
    /// モデル通知登録
    /// </summary>
    private void Bind(){

        // 設置アイテムに追加時の処理
        collectItemModel.PutCollectItemAddObservable.Subscribe(collectItem => {
            CreateCollectItem(collectItem.Value);
        })
        .AddTo(this);

    }

    /// <summary>
    /// アイテムの生成
    /// </summary>
    /// <param name="collectItemId"></param>
    private void CreateCollectItem(MstAdvCollectItemData collectItemData) {

        var item = advCollectItemFactory.CreateCollectItem(collectItemData.ID);

        item.Init(collectItemData);

        // アイテム取得時の通知監視
        item.OnGetObservable.Subscribe(item => {

            // 通知を表示した後に表示リストから削除            
            ShowNotification(item);
            displayCollectItems.Remove(item.ID);
        });

        displayCollectItems[collectItemData.ID] = item;
    }

    /// <summary>
    /// 通知の表示
    /// </summary>
    /// <param name="collectItemId"></param>
    private void ShowNotification(MstAdvCollectItemData collectItem){

        advCollectNotification.Init(collectItem.Name);


    }
}
