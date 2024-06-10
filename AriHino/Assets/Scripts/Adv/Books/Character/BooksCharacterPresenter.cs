using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Linq;
using Cysharp.Text;

public class BooksCharacterPresenter : MonoBehaviour
{
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject prevPageButton;
    [SerializeField] private BooksCharacterPageView pageViewPrefab;
    private Dictionary<int, BooksCharacterPageView> pageViewDict = new();
    private IBooksCharacterModel booksCharacterModel;
    private BooksPageButtonHandler booksButtonHandler;
    private int indexNow;

    private CompositeDisposable disposables = new();

    public void Init(IBooksCharacterModel model, BooksPageButtonHandler buttonHandler) {

        ResetPresenter();

        booksCharacterModel = model;
        booksButtonHandler = buttonHandler;
        indexNow = model.DisplayCharacterPageDict.First().Key;
        CreateCharacterPage();
       
        Bind();
    }

    private void Bind() {

        booksCharacterModel.DisplayCharacterPageDict.ObserveAdd().Subscribe(page => {

            var view = Instantiate(pageViewPrefab);
            view.Init(page.Value);
            pageViewDict.Add(page.Value.CharaId, view);
            DisplayUpdate();

            string notificationFormat = "{0}の情報を追加しました";
            string notificationText = ZString.Format(notificationFormat, page.Value.CharaName);
            NotificationManager.Instance.ShowNotification(notificationText);

        }).AddTo(disposables);

        booksCharacterModel.DisplayCharacterPageDict.ObserveRemove().Subscribe(page => {

            if(pageViewDict.TryGetValue(page.Value.CharaId, out BooksCharacterPageView view)) {
                pageViewDict.Remove(page.Value.CharaId);
                Destroy(view);
                DisplayUpdate();
            }

        }).AddTo(disposables);

        booksCharacterModel.DisplayCharacterPageDict.ObserveReset().Subscribe(_ => {

            var destroyObjs = new List<BooksCharacterPageView>(pageViewDict.Values);
            pageViewDict.Clear();

            foreach(var obj in destroyObjs) {
                Destroy(obj.gameObject);
            }

        }).AddTo(disposables);

        booksCharacterModel.DisplayCharacterPageDict.ObserveReplace().Subscribe(page => {

            pageViewDict[page.NewValue.CharaId].Init(page.NewValue);
            DisplayUpdate();

            string notificationFormat = "{0}の情報を更新しました";
            string notificationText = ZString.Format(notificationFormat, page.NewValue.CharaName);
            NotificationManager.Instance.ShowNotification(notificationText);

        }).AddTo(disposables);
    }

    private void CreateCharacterPage() {

        foreach(var page in booksCharacterModel.DisplayCharacterPageDict) {
            var view = Instantiate(pageViewPrefab, this.transform);
            view.Init(page.Value);
            pageViewDict.Add(page.Value.CharaId, view);
        }

        pageViewDict[indexNow].gameObject.SetActive(true);
    }

    private void DisplayUpdate() {
        nextPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow < e));
        prevPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow > e));

        booksCharacterModel.ReadedPageDic[pageViewDict[indexNow].PageViewData.CharaId] = true;
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

    private void ResetPresenter() {
        
        var destroyObjs = new List<BooksCharacterPageView>(pageViewDict.Values);
        pageViewDict.Clear();

        foreach(var obj in destroyObjs) {
            Destroy(obj.gameObject);
        }

        disposables.Clear();

    }

    public void ShowPage() {

        DisplayUpdate();
    }
}
