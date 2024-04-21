using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Linq;

public class BooksCharacterPresenter : MonoBehaviour
{
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject prevPageButton;
    [SerializeField] private BooksCharacterPageView pageViewPrefab;
    private Dictionary<int, BooksCharacterPageView> pageViewDict = new();
    private IBooksCharacterModel booksCharacterModel;
    private BooksButtonHandler booksButtonHandler;
    private int indexNow;

    public void Init(BooksCharacterModel model, BooksButtonHandler buttonHandler) {
        this.booksCharacterModel = model;
        booksButtonHandler = buttonHandler;

        CreateCharacterPage();
        Bind();

        indexNow = pageViewDict.Keys.OrderBy(e => e).First();

        DisplayUpdate();
    }

    private void Bind() {

        booksCharacterModel.DisplayCharacterPageAddObservable.Subscribe(page => {
            
            var pageViewData = new BooksCharacterPageViewData(page.Value);
            var view = Instantiate(pageViewPrefab);
            view.Init(pageViewData);
            pageViewDict.Add(pageViewData.ID, view);
            DisplayUpdate();

        }).AddTo(this);

        booksCharacterModel.DisplayCharacterPageRemoveObservable.Subscribe(page => {
            pageViewDict.Remove(page.Value.ID);
            DisplayUpdate();

        }).AddTo(this);
    }

    private void CreateCharacterPage() {

        foreach(var page in booksCharacterModel.DisplayCharacterPageDict) {
            var pageViewData = new BooksCharacterPageViewData(page.Value);
            var view = Instantiate(pageViewPrefab, this.transform);
            view.Init(pageViewData);
            view.gameObject.SetActive(true);
            pageViewDict.Add(pageViewData.ID, view);
        }
    }

    private void DisplayUpdate() {
        nextPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow < e));
        prevPageButton.SetActive(pageViewDict.Keys.Any(e => indexNow > e));
    }

    public void OnNextPage() {

        foreach (var page in pageViewDict) page.Value.gameObject.SetActive(false);
        int nextKey = pageViewDict.Keys.OrderBy(e => e).SkipWhile(e => e <= indexNow).First();
        indexNow = nextKey;

        UniTask.Void(async() => {
            await booksButtonHandler.PlayPageSingleAnim(true);
            pageViewDict[indexNow].gameObject.SetActive(true);
        });
    }

    public void OnPrevButton() {
        foreach (var page in pageViewDict) page.Value.gameObject.SetActive(false);
        int prevKey = pageViewDict.Keys.OrderByDescending(e => e).SkipWhile(e => e >= indexNow).First();
        indexNow = prevKey;

        UniTask.Void(async() => {
            await booksButtonHandler.PlayPageSingleAnim(false);
            pageViewDict[indexNow].gameObject.SetActive(true);
        });
    }


}
