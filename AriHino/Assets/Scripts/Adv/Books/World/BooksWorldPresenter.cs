using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Linq;
using Cysharp.Text;

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

    public void Init(IBooksWorldModel model, BooksPageButtonHandler buttonHandler) {
        this.booksWorldModel = model;
        booksButtonHandler = buttonHandler;

        indexNow = model.DisplayWorldPageDict.First().Key;

        CreateWorldPage();
        Bind();
        DisplayUpdate();
    }

    private void Bind() {

        booksWorldModel.DisplayWorldPageDict.ObserveAdd().Subscribe(page => {
            
            var view = Instantiate(pageViewPrefab);
            view.Init(page.Value);
            pageViewDict.Add(page.Value.WorldId, view);
            DisplayUpdate();

            string notificationFormat = "世界情報を追加しました";
            //string notificationText = ZString.Format(notificationFormat, page.NewValue.WorldName);
            NotificationManager.Instance.ShowNotification(notificationFormat);

        }).AddTo(this);

        booksWorldModel.DisplayWorldPageDict.ObserveRemove().Subscribe(page => {
            if(pageViewDict.TryGetValue(page.Value.WorldId, out BooksWorldPageView view)) {
                pageViewDict.Remove(page.Value.WorldId);
                Destroy(view);
                DisplayUpdate();
            }

        }).AddTo(this);

        booksWorldModel.DisplayWorldPageDict.ObserveReset().Subscribe(_ => {
            var destroyObjs = new List<BooksWorldPageView>(pageViewDict.Values);
            pageViewDict.Clear();

            foreach(var obj in destroyObjs) {
                Destroy(obj);
            }
        }).AddTo(this);

        booksWorldModel.DisplayWorldPageDict.ObserveReplace().Subscribe(page => {

            pageViewDict[page.NewValue.WorldId].Init(page.NewValue);
            DisplayUpdate();

            string notificationFormat = "世界情報を更新しました";
            //string notificationText = ZString.Format(notificationFormat, page.NewValue.WorldName);
            NotificationManager.Instance.ShowNotification(notificationFormat);
        }).AddTo(this);
    }

    private void CreateWorldPage() {

        foreach(var page in booksWorldModel.DisplayWorldPageDict) {
            var view = Instantiate(pageViewPrefab, page.Value.WorldId % 2 == 0 ? rightPageParent : leftPageParent);
            view.Init(page.Value);
            pageViewDict.Add(page.Value.WorldId, view);
        }

        var viewPage = pageViewDict.Values.Take(2);
        foreach(var page in viewPage) { page.gameObject.SetActive(true); }
    }

    private void DisplayUpdate() {
        nextPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow + 1 < e));
        prevPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow - 1 > e));
    }

    public void OnNextPage() {

        if(booksButtonHandler.IsPlayingAnim) return;

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

        if(booksButtonHandler.IsPlayingAnim) return;

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
