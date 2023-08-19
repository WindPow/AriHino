using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class AdvCollectItemGroup : MonoBehaviour
{
    [SerializeField] private AdvCollectItemDataView[] collectItemDataViews;

    private Subject<AdvCollectItemData> onGetSubject = new Subject<AdvCollectItemData>();
    public IObservable<AdvCollectItemData> OnGetObservable => onGetSubject;

    public void Init(){

        foreach(var collectItem in collectItemDataViews){

            collectItem.OnGetObservable.Subscribe(data => {
                onGetSubject.OnNext(data);
            });

        }

    }
}
