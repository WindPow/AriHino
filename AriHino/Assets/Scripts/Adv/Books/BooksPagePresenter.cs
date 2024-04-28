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
    [SerializeField] AdvCollectItemPresenter advCollectItemPresenter;
    [SerializeField] BooksPageButtonHandler booksButtonHandler;

    private IBooksPageModel booksPageModel;
    private IBooksCharacterModel booksCharacterModel;
    private IBooksWorldModel booksWorldModel;
    private IBooksWardModel booksWardModel;
    private IBooksCollectModel booksCollectModel;

    public void Init(IBooksPageModel pageModel, IBooksCharacterModel characterModel, IBooksWorldModel worldModel, IBooksWardModel wardModel, IBooksCollectModel collectModel) {
        
        booksPageModel = pageModel;

        booksCharacterModel = characterModel;
        if(booksCharacterPresenter) booksCharacterPresenter.Init(booksCharacterModel, booksButtonHandler);

        booksWorldModel = worldModel;
        if(booksWorldPresenter) booksWorldPresenter.Init(booksWorldModel, booksButtonHandler);

        booksCollectModel = collectModel;
        if(booksCollectPresenter) booksCollectPresenter.Init(booksCollectModel, booksButtonHandler);

    }

    public void Bind() {

        booksButtonHandler.ChangeIndexObservable.Subscribe(index => {
            ChangePage(index);
        }).AddTo(this);

        booksPageModel.BooksData.Subscribe(data => {

            booksCharacterModel.SetBooksCharacter(data.CharacterPageIds);
            booksWorldModel.SetBooksWorld(data.WorldPageIds);

        }).AddTo(this);
    }

    private void ChangePage(int pageIndex){

        switch((BooksContents)pageIndex){

            case BooksContents.Character:

                //booksCharacterPresenter
                break;
            case BooksContents.World:
                break;
            case BooksContents.Ward:
                break;
            case BooksContents.Collect:
                break;
        }

    }
}

public enum BooksContents {
    Character = 0,
    World = 1,
    Ward = 2,
    Collect = 3
}
