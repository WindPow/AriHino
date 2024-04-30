using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BooksManager : MonoBehaviour
{
    [SerializeField] private BooksPagePresenter booksPagePresenter;
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
    }

    public void SetBooks(int booksId) {
        var mstBooks = MasterDataManager.Instance.GetMasterData<MstBooksData>(booksId);
        booksPageModel.SetBooksData(mstBooks);
    }

    public void SetBooksCharacterPage(int charaId, int progressNo) {
        var booksCharaPageDict = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksCharacterPageData>();
        var setCharacterPage = booksCharaPageDict.Values.First(e => e.CharaId == charaId && e.ProgressNo == progressNo);
        booksCharacterModel.SetBooksCharacter(setCharacterPage);
    }

    public void SetBooksWorldPage(int worldId, int progressNo) {
        var booksWorldPageDict = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksWorldPageData>();
        var setWorldPage = booksWorldPageDict.Values.First(e => e.WorldId == worldId && e.ProgressNo == progressNo);
        booksWorldModel.SetBooksWorld(setWorldPage);
    }

    public void SetBooksCollectItem(MstCollectItemData collectItemData) {
        booksCollectModel.OpenBooksCollect(collectItemData);
    }
}
