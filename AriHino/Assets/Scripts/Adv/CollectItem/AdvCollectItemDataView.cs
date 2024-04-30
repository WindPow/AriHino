using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

/// <summary>
/// 画面に表示するコレクトアイテムの表示物
/// </summary>
public interface IAdvCollectItemDataView{

    IObservable<MstCollectItemData> OnGetObservable { get; }

    void Init(MstCollectItemData itemData);
}


public class AdvCollectItemDataView : MonoBehaviour, IAdvCollectItemDataView
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
