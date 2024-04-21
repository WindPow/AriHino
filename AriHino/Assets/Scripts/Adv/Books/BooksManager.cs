using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksManager : MonoBehaviour
{
    [SerializeField] private BooksPagePresenter booksPagePresenter;
    private BooksPageModel booksPageModel;

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
        booksPagePresenter.Init(booksPageModel);
    }

    public void ChangeBooks(int booksId) {
        var mstBooks = MasterDataManager.Instance.GetMasterData<MstBooksData>(booksId);
        booksPageModel.SetBooksData(mstBooks);
    }

    public void ChangeBooksCharacter(int booksCharacterId) {
        var mstBooksCharacter = MasterDataManager.Instance.GetMasterData<MstBooksCharacterPageData>(booksCharacterId);

    }
}
