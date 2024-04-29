using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class BooksCollectContentsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    private BooksCollectContentsViewData collectItemData;

    public void Init(BooksCollectContentsViewData data) {

        collectItemData = data;
        nameText.text = data.MstCollectItemData.Name;
        nameText.gameObject.SetActive(data.IsOpen);

        collectItemData.SetOpenObservable.Subscribe(isOpen => {
            nameText.gameObject.SetActive(isOpen);
        }).AddTo(this);
    }

    public void OnClick(Action<BooksCollectContentsViewData> clickAction) {

        clickAction?.Invoke(collectItemData);
    }
}
