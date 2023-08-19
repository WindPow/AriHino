using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class AdvCollectItemPresenter : MonoBehaviour
{
    [SerializeField] private AdvCollectNotification advCollectNotification;
    
    private IAdvCollectItemModel collectItemModel;

    private IAdvCollectItemFactory collectItemFactory;

    private Dictionary<int, AdvCollectItemGroup> collectItems = new Dictionary<int, AdvCollectItemGroup>();

    public void Init(AdvCollectItemModel collectItemModel){

        this.collectItemModel = collectItemModel;

        collectItemFactory = new AdvCollectItemFactory();

        Bind();

    }

    private void Bind(){

        collectItemModel.CollectItemGroupId.Subscribe(collectItemId => {
            CreateCollectItem(collectItemId);
        })
        .AddTo(this);

    }

    private void CreateCollectItem(int collectItemId) {

        var item = collectItemFactory.CreateCollectItem(collectItemId);

        item.Init();

        item.OnGetObservable.Subscribe(item => {
            
        });

        collectItems[collectItemId] = item;
    }

    private void ShowNotification(int collectItemId){

        advCollectNotification

    }
}
