using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksCollectPageView : MonoBehaviour
{
    [SerializeField] private Transform ContentsRoot;
    [SerializeField] private BooksCollectContentsView contentsPrefab;
    [SerializeField] private AdvCollectItemDetailDialog detailPrefab;

    private Transform detailRoot;

    public void Init(List<BooksCollectContentsViewData> collectItemDatas, Transform detailRoot) {

        CreateCollectItems(collectItemDatas);
        this.detailRoot = detailRoot;
    }

    private void CreateCollectItems(List<BooksCollectContentsViewData> collectItemDatas) {

        foreach (BooksCollectContentsViewData item in collectItemDatas) {
            var view = Instantiate(contentsPrefab, ContentsRoot);
            view.Init(item, item => {
                ShowDetailDialog(item.MstCollectItemData);
            });   
        }
    }

    private void ShowDetailDialog(MstCollectItemData mstCollectItemData) {

        var detail = Instantiate(detailPrefab, detailRoot);
        detail.Init(mstCollectItemData);
    }
}
