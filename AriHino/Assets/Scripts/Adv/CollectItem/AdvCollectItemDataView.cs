using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class AdvCollectItemDataView : MonoBehaviour
{
    [SerializeField] private int collectItemId;
    [SerializeField] private Image image;

    private AdvCollectItemData itemData;

    public IObservable<AdvCollectItemData> OnGetObservable => onGetSubject;
    private readonly Subject<AdvCollectItemData> onGetSubject = new();

    // Start is called before the first frame update
    void Start()
    {
        itemData = new AdvCollectItemData(collectItemId);
    }

    public void Init(){

        image = null;

        //TODO:表示の更新を行う
    }

    public void OnClick(){

        onGetSubject.OnNext(itemData);

        // 一旦削除（エフェクトなどを発生させる可能性あり）
        Destroy(this.gameObject);
    }
}
