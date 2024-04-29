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
    private BooksPageButtonHandler booksButtonHandler;
    private int indexNow;

    public void Init(IBooksCharacterModel model, BooksPageButtonHandler buttonHandler) {
        booksCharacterModel = model;
        booksButtonHandler = buttonHandler;
        indexNow = model.DisplayCharacterPageDict.First().Key;

        CreateCharacterPage();
        Bind();
        DisplayUpdate();
    }

    private void Bind() {

        booksCharacterModel.DisplayCharacterPageDict.ObserveAdd().Subscribe(page => {

            var view = Instantiate(pageViewPrefab);
            view.Init(page.Value);
            pageViewDict.Add(page.Value.CharaId, view);
            DisplayUpdate();

        }).AddTo(this);

        booksCharacterModel.DisplayCharacterPageDict.ObserveRemove().Subscribe(page => {

            pageViewDict.Remove(page.Value.CharaId);
            DisplayUpdate();

        }).AddTo(this);

        booksCharacterModel.DisplayCharacterPageDict.ObserveReplace().Subscribe(page => {

            pageViewDict[page.NewValue.CharaId].Init(page.NewValue);
            DisplayUpdate();

            NotificationManager.Instance.ShowNotification(page.NewValue.CharaName);

        }).AddTo(this);
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
