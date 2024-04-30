using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IBooksPageModel {

    IReadOnlyReactiveProperty<MstBooksData> BooksData { get; }

    void SetBooksData(MstBooksData booksData);
}

public class BooksPageModel : IBooksPageModel
{
    public IReadOnlyReactiveProperty<MstBooksData> BooksData => booksData;
    private ReactiveProperty<MstBooksData> booksData = new();

    public BooksPageModel(MstBooksData booksData) {

        this.booksData.Value = booksData;
        //TODO:Saveデータからリストを取得
    }

    public void SetBooksData(MstBooksData booksData) {

        this.booksData.Value = booksData;

    }
}
