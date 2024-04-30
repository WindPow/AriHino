using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Linq;
using System;

public class BooksCollectPresenter : MonoBehaviour
{
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject prevPageButton;
    [SerializeField] private BooksCollectPageView pageViewPrefab;
    private Dictionary<int, BooksCollectPageView> pageViewDict = new();
    private IBooksCollectModel booksCollectModel;
    private BooksPageButtonHandler booksButtonHandler;
    private int indexNow;

    public void Init(IBooksCollectModel model, BooksPageButtonHandler buttonHandler) {
        booksCollectModel = model;
        booksButtonHandler = buttonHandler;

        CreateCollectPage();
        Bind();
        DisplayUpdate();
    }

    private void Bind() {

        // なにかバインドしたいときここつかえ
    }

    private void CreateCollectPage() {

        var dividedLists = booksCollectModel.DisplayCollectPageDict.Values
                .Select((value, index) => new { value, index })
                .GroupBy(e => e.index / 10)
                .Select(e => e.Select(x => x.value).ToList())
                .ToList();

        int count = 0;
        foreach (var dividedList in dividedLists) {
            var pageView = Instantiate(pageViewPrefab, this.transform);
            pageView.Init(dividedList);
            pageViewDict.Add(count, pageView);
            count++;
        }

        pageViewDict[indexNow].gameObject.SetActive(true);
    }

    private void DisplayUpdate() {
        nextPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow < e));
        prevPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow > e));
    }

    public void OnNextPage() {

        if(booksButtonHandler.IsPlayingAnim) return;

        foreach (var page in pageViewDict) page.Value.gameObject.SetActive(false);
        int nextKey = pageViewDict.Keys.OrderBy(e => e).SkipWhile(e => e <= indexNow).First();
        pageViewDict[indexNow].gameObject.SetActive(false);
        indexNow = nextKey;

        UniTask.Void(async() => {
            await booksButtonHandler.PlayPageSingleAnim(true);
            pageViewDict[indexNow].gameObject.SetActive(true);
            DisplayUpdate();
        });
    }

    public void OnPrevButton() {

        if(booksButtonHandler.IsPlayingAnim) return;

        foreach (var page in pageViewDict) page.Value.gameObject.SetActive(false);
        int prevKey = pageViewDict.Keys.OrderByDescending(e => e).SkipWhile(e => e >= indexNow).First();
        pageViewDict[indexNow].gameObject.SetActive(false);
        indexNow = prevKey;

        UniTask.Void(async() => {
            await booksButtonHandler.PlayPageSingleAnim(false);
            pageViewDict[indexNow].gameObject.SetActive(true);
            DisplayUpdate();
        });
    }


}
