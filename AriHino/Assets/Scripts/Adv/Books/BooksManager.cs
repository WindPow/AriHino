using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksManager : MonoBehaviour
{
    [SerializeField] private BooksPagePresenter booksPagePresenter;
    [SerializeField] private GameObject booksButton;
    private IBooksPageModel booksPageModel;
    private IBooksCharacterModel booksCharacterModel;
    private IBooksWorldModel booksWorldModel;
    private IBooksWardModel booksWardModel;
    private IBooksCollectModel booksCollectModel;

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

        // 最初はBooksボタンを封印しておく
        ChangeBooksOpen(false);
    }

    public void ChangeBooksOpen(bool isOpen) {
        booksButton.SetActive(isOpen);
    }

    public void UpdateBooks(int booksId) {
        var mstBooks = MasterDataManager.Instance.GetMasterData<MstBooksData>(booksId);
        booksPageModel.SetBooksData(mstBooks);
    }

    public void SetBooksCollectItem(MstCollectItemData collectItemData) {
        booksCollectModel.OpenBooksCollect(collectItemData);
    }
}
