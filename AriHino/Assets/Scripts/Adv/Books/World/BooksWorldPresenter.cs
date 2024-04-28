using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Linq;

public class BooksWorldPresenter : MonoBehaviour
{
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject prevPageButton;
    [SerializeField] private Transform leftPageParent;
    [SerializeField] private Transform rightPageParent;
    [SerializeField] private BooksWorldPageView pageViewPrefab;
    private Dictionary<int, BooksWorldPageView> pageViewDict = new();
    private IBooksWorldModel booksWorldModel;
    private BooksPageButtonHandler booksButtonHandler;
    private int indexNow;
    private int displayPageIndex;

    public void Init(IBooksWorldModel model, BooksPageButtonHandler buttonHandler) {
        this.booksWorldModel = model;
        booksButtonHandler = buttonHandler;

        indexNow = model.DisplayWorldPageDict.Keys.First();

        CreateWorldPage();
        Bind();
        DisplayUpdate();
    }

    private void Bind() {

        booksWorldModel.DisplayWorldPageAddObservable.Subscribe(page => {
            
            var pageViewData = new BooksWorldPageViewData(page.Value);
            var view = Instantiate(pageViewPrefab);
            view.Init(pageViewData);
            pageViewDict.Add(pageViewData.ID, view);
            DisplayUpdate();

        }).AddTo(this);

        booksWorldModel.DisplayWorldPageRemoveObservable.Subscribe(page => {
            pageViewDict.Remove(page.Value.ID);
            DisplayUpdate();

        }).AddTo(this);
    }

    private void CreateWorldPage() {

        foreach(var page in booksWorldModel.DisplayWorldPageDict) {
            var pageViewData = new BooksWorldPageViewData(page.Value);
            var view = Instantiate(pageViewPrefab, pageViewData.ID % 2 == 0 ? rightPageParent : leftPageParent);
            view.Init(pageViewData);
            pageViewDict.Add(pageViewData.ID, view);
        }

        var viewPage = pageViewDict.Values.Take(2);
        foreach(var page in viewPage) { page.gameObject.SetActive(true); }
    }

    private void DisplayUpdate() {
        nextPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow + 1 < e));
        prevPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow - 1 > e));
    }

    public void OnNextPage() {

        foreach (var page in pageViewDict) page.Value.gameObject.SetActive(false);
        int nextKey = pageViewDict.Keys.OrderBy(e => e).SkipWhile(e => e <= indexNow + 1).First();
        indexNow = nextKey;

        UniTask.Void(async() => {
            await booksButtonHandler.PlayPageSingleAnim(true);
            pageViewDict[indexNow].gameObject.SetActive(true);
            pageViewDict[indexNow + 1].gameObject.SetActive(true);

            var inactiveViews = pageViewDict.Where(e => e.Key != indexNow && e.Key != indexNow + 1);
            foreach(var page in inactiveViews) page.Value.gameObject.SetActive(false);

            DisplayUpdate();

        });
    }

    public void OnPrevButton() {
        foreach (var page in pageViewDict) page.Value.gameObject.SetActive(false);
        int prevKey = pageViewDict.Keys.OrderByDescending(e => e).SkipWhile(e => e >= indexNow - 1).First();
        indexNow = prevKey;

        UniTask.Void(async() => {
            await booksButtonHandler.PlayPageSingleAnim(false);
            pageViewDict[indexNow].gameObject.SetActive(true);
            pageViewDict[indexNow + 1].gameObject.SetActive(true);

            var inactiveViews = pageViewDict.Where(e => e.Key != indexNow && e.Key != indexNow + 1);
            foreach(var page in inactiveViews) page.Value.gameObject.SetActive(false);

            DisplayUpdate();
        });
    }


}
