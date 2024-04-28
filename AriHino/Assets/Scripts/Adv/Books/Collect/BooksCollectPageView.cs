using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksCollectPageView : MonoBehaviour
{
    [SerializeField] private Transform ContentsRoot;
    [SerializeField] private BooksCollectContentsView contentsPrefab;

    public void Init(List<BooksCollectContentsViewData> collectItemDatas) {

        CreateCollectItems(collectItemDatas);
    }

    private void CreateCollectItems(List<BooksCollectContentsViewData> collectItemDatas) {

        foreach (BooksCollectContentsViewData item in collectItemDatas) {
            var view = Instantiate(contentsPrefab, ContentsRoot);
            view.Init(item);
        }
        
    }
}
