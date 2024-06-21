using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Linq;

public class BooksPagePresenter : MonoBehaviour
{
    [SerializeField] BooksCharacterPresenter booksCharacterPresenter;
    [SerializeField] BooksWorldPresenter booksWorldPresenter;
    [SerializeField] BooksWardPresenter booksWardPresenter;
    [SerializeField] BooksCollectPresenter booksCollectPresenter;
    [SerializeField] BooksPageButtonHandler booksButtonHandler;

    private IBooksPageModel booksPageModel;
    private IBooksCharacterModel booksCharacterModel;
    private IBooksWorldModel booksWorldModel;
    private IBooksWardModel booksWardModel;
    private IBooksCollectModel booksCollectModel;

    private BooksPage nowPage;

    public void Init(IBooksPageModel pageModel, IBooksCharacterModel characterModel, IBooksWorldModel worldModel, IBooksWardModel wardModel, IBooksCollectModel collectModel) {
        
        booksPageModel = pageModel;

        booksButtonHandler.Init();

        booksCharacterModel = characterModel;
        if(booksCharacterPresenter) booksCharacterPresenter.Init(booksCharacterModel, booksButtonHandler);

        booksWorldModel = worldModel;
        if(booksWorldPresenter) booksWorldPresenter.Init(booksWorldModel, booksButtonHandler);

        booksCollectModel = collectModel;
        if(booksCollectPresenter) booksCollectPresenter.Init(booksCollectModel, booksButtonHandler);

        Bind();

        ChangePage(0);
    }

    private void Bind() {

        booksButtonHandler.ChangeIndexObservable.Subscribe(index => {
            ChangePage(index);
        }).AddTo(this);

        booksPageModel.BooksData.Subscribe(data => {

            //booksCharacterModel.SetBooksCharacter(data.CharacterPageIds);
            //booksWorldModel.SetBooksWorld(data.WorldPageIds);

        }).AddTo(this);
    }

    private void ChangePage(int pageIndex) {

        switch((BooksPage)pageIndex){

            case BooksPage.Character:
                booksCharacterPresenter.ShowPage();
                nowPage = (BooksPage)pageIndex;
                break;
            case BooksPage.World:
                booksWorldPresenter.ShowPage();
                nowPage = (BooksPage)pageIndex;
                break;
            case BooksPage.Ward:
                nowPage = (BooksPage)pageIndex;
                break;
            case BooksPage.Collect:
                booksCollectPresenter.ShowPage();
                nowPage = (BooksPage)pageIndex;
                break;
        }
    }

    public void ShowBooks() {
        booksButtonHandler.OnTapStikcyNote((int)nowPage);
    }
}

public enum BooksPage {
    Character = 0,
    World = 1,
    Ward = 2,
    Collect = 3
}
