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
    
    private IAdvCollectItemModel collectItemModel;

    private Dictionary<int, AdvCollectItemDataView> displayCollectItems = new Dictionary<int, AdvCollectItemDataView>();

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
        collectItemModel.PutCollectItems.ObserveAdd().Subscribe(collectItem => {
            CreateCollectItem(collectItem.Value);
        })
        .AddTo(this);

        // オブジェクト削除処理
        collectItemModel.PutCollectItems.ObserveRemove().Subscribe(collectItem => {

            if(displayCollectItems.TryGetValue(collectItem.Value.ID, out AdvCollectItemDataView removeItem)){
                displayCollectItems.Remove(collectItem.Value.ID);
                Destroy(removeItem.gameObject);
            }
        }).AddTo(this);

        collectItemModel.PutCollectItems.ObserveReset().Subscribe(_ => {
            var destroyObjs = new List<AdvCollectItemDataView>(displayCollectItems.Values);
            displayCollectItems.Clear();

            foreach(var obj in destroyObjs) {
                Destroy(obj);
            }

        }).AddTo(this);

    }

    /// <summary>
    /// アイテムの生成
    /// </summary>
    /// <param name="collectItemId"></param>
    private void CreateCollectItem(MstCollectItemData collectItemData) {

        var item = advCollectItemFactory.CreateCollectItem(collectItemData.ID);

        item.Init(collectItemData);

        // アイテム取得時の通知監視
        item.OnGetObservable.Subscribe(item => {

            BooksManager.Instance.SetBooksCollectItem(item);

            // 通知を表示した後に表示リストから削除 
            NotificationManager.Instance.ShowNotification(item.Name);

            collectItemModel.RemoveCollectItem(item.ID);
        });

        displayCollectItems[collectItemData.ID] = item;
    }
}
