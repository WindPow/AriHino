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

    IObservable<MstAdvCollectItemData> OnGetObservable { get; }

    void Init(MstAdvCollectItemData itemData);
}


public class AdvCollectItemDataView : MonoBehaviour, IAdvCollectItemDataView
{

    [SerializeField] private int collectItemId;

    private MstAdvCollectItemData itemData;

    public IObservable<MstAdvCollectItemData> OnGetObservable => onGetSubject;
    private readonly Subject<MstAdvCollectItemData> onGetSubject = new();

    /// <summary>
    /// クリックした時に呼ばれる
    /// </summary>
    public void OnClick(){

        onGetSubject.OnNext(itemData);

        // 一旦削除（エフェクトなどを発生させる可能性あり）
        Destroy(this.gameObject);
    }

    public void Init(MstAdvCollectItemData itemData)
    {

        throw new NotImplementedException();
    }
}
