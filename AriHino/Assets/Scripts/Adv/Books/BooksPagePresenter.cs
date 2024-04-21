using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

public class BooksPagePresenter : MonoBehaviour
{
    [SerializeField] BooksCharacterPresenter booksCharacterPresenter;
    [SerializeField] BooksWorldPresenter booksWorldPresenter;
    [SerializeField] BooksWardPresenter booksWardPresenter;
    [SerializeField] BooksCollectPresenter booksCollectPresenter;
    [SerializeField] AdvCollectItemPresenter advCollectItemPresenter;
    [SerializeField] BooksButtonHandler booksButtonHandler;

    private BooksPageModel booksPageModel;

    private BooksCharacterModel booksCharacterModel;
    private BooksWorldModel booksWorldModel;
    private BooksWardModel booksWardModel;
    private BooksCollectModel booksCollectModel;

    public void Init(BooksPageModel model) {
        
        booksPageModel = model;

        booksCharacterModel = new BooksCharacterModel(model.BooksData.Value.CharacterPageIds);
        booksCharacterPresenter.Init(booksCharacterModel, booksButtonHandler);

    }

    public void Bind() {

        booksButtonHandler.ChangeIndexObservable.Subscribe(index => {
            ChangePage(index);
        }).AddTo(this);

        booksPageModel.BooksData.Subscribe(data => {

            booksCharacterModel.SetBooksCharacter(data.CharacterPageIds);
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
