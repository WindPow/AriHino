using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class AdvCollectItemDataView : MonoBehaviour
{

    [SerializeField] private int collectItemId;

    private MstCollectItemData itemData;

    public IObservable<MstCollectItemData> OnGetObservable => onGetSubject;
    private readonly Subject<MstCollectItemData> onGetSubject = new();

    public void Init(MstCollectItemData itemData)
    {
        this.itemData = itemData;
    }

    /// <summary>
    /// クリックした時に呼ばれる
    /// </summary>
    public void OnClick(){

        onGetSubject.OnNext(itemData);

        // 一旦削除（エフェクトなどを発生させる可能性あり）
        Destroy(this.gameObject);
    }
}
