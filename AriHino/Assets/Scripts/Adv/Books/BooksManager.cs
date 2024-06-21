using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Utage;

public class BooksManager : MonoBehaviour
{
    [SerializeField] private BooksPagePresenter booksPagePresenter;
    [SerializeField] private GameObject booksButton;
    [SerializeField] private AdvUIHandler advUIHandler;
    private IBooksPageModel booksPageModel;
    private IBooksCharacterModel booksCharacterModel;
    private IBooksWorldModel booksWorldModel;
    private IBooksWardModel booksWardModel;
    private IBooksCollectModel booksCollectModel;

    /// <summary>
    /// Booksが表示状態かのフラグ
    /// </summary>
    public bool IsOpenBooks { get; private set; }

    private static BooksManager instance;
    public static BooksManager Instance {

        get {
            if (instance == null) instance = GameObject.FindObjectOfType<BooksManager>();

            if (instance == null) {
                GameObject singletonObject = new GameObject(typeof(BooksManager).Name);
                instance = singletonObject.AddComponent<BooksManager>();
            }
            return instance;
        }
    }

    public void Init() {

        var mstBooks = MasterDataManager.Instance.GetMasterData<MstBooksData>(1);

        booksPageModel = new BooksPageModel(mstBooks);
        booksCharacterModel = new BooksCharacterModel(mstBooks.CharacterPageIds);
        booksWorldModel = new BooksWorldModel(mstBooks.WorldPageIds);
        booksCollectModel = new BooksCollectModel();

        booksPagePresenter.Init(booksPageModel, booksCharacterModel, booksWorldModel, booksWardModel, booksCollectModel);

        Bind();
    }

    private void Bind() {

        // Booksの未読があれバッジを表示する
        booksCharacterModel.ReadedPageDic.ObserveReplace().Subscribe(e => {
            BooksReadUpdate();
        }).AddTo(this);

        booksCharacterModel.ReadedPageDic.ObserveAdd().Subscribe(e => {
            BooksReadUpdate();
        }).AddTo(this);

        booksWorldModel.ReadedPageDic.ObserveReplace().Subscribe(e => {
            BooksReadUpdate();
        }).AddTo(this);

        booksWorldModel.ReadedPageDic.ObserveAdd().Subscribe(e => {
            BooksReadUpdate();
        }).AddTo(this);

        booksCollectModel.ReadedContentsDic.ObserveReplace().Subscribe(e => {
            BooksReadUpdate();
        }).AddTo(this);

        booksCollectModel.ReadedContentsDic.ObserveAdd().Subscribe(e => {
            BooksReadUpdate();
        }).AddTo(this);
    }

    private void BooksReadUpdate() {

        if(booksCharacterModel.ReadedPageDic.Values.Any(e => e == false) 
            || booksWorldModel.ReadedPageDic.Values.Any(e => e == false)
            || booksCollectModel.ReadedContentsDic.Values.Any(e => e == false)) 
        {
            advUIHandler.ChangeBooksNewBadge(true);
        }
        else {
            advUIHandler.ChangeBooksNewBadge(false);
        }
    }

    public void SetIsOpenBooks(bool isOpen) {
        IsOpenBooks = isOpen;
        if(isOpen) booksPagePresenter.ShowBooks();
    }
    
    public void ActivateBooks(bool isOpen) {
        advUIHandler.ActivateBooks(!isOpen);
        IsOpenBooks = isOpen;
    }

    #region Command

    public void SetBooks(int booksId) {
        var mstBooks = MasterDataManager.Instance.GetMasterData<MstBooksData>(booksId);
        booksPageModel.SetBooksData(mstBooks);
    }

    public void SetBooksCharacterPage(int charaId, int progressNo) {
        var booksCharaPageDict = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksCharacterPageData>();
        var setCharacterPage = booksCharaPageDict.Values.First(e => e.CharaId == charaId && e.ProgressNo == progressNo);
        booksCharacterModel.SetBooksCharacter(setCharacterPage);
    }

    public void RemoveBooksCharacterPage(int characterId) {
        booksCharacterModel.RemoveBooksCharacter(characterId);
    }

    public void SetBooksWorldPage(int worldId, int progressNo) {
        var booksWorldPageDict = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksWorldPageData>();
        var setWorldPage = booksWorldPageDict.Values.First(e => e.WorldId == worldId && e.ProgressNo == progressNo);
        booksWorldModel.SetBooksWorld(setWorldPage);
    }

    public void RemoveBooksWorldPage(int worldId) {
        booksWorldModel.RemoveBooksWorld(worldId);
    }

    public void SetBooksCollectItem(MstCollectItemData collectItemData) {
        booksCollectModel.OpenBooksCollect(collectItemData);
    }

    public void ActiveBooksButton(bool isOpen) {
        booksButton.SetActive(isOpen);
        IsOpenBooks = isOpen;

        if(isOpen) {
            string notificationText = LanguageManager.Instance.CurrentLanguage == "Japanese" ? "手記が使用可能になりました" : "Journal is now available";
            NotificationManager.Instance.ShowNotification(notificationText);
        }
        else { 
            string notificationText = LanguageManager.Instance.CurrentLanguage == "Japanese" ? "手記が使用不能になりました": "Journal is now unavailable";
            NotificationManager.Instance.ShowNotification(notificationText);
        }
    }

    #endregion

}
