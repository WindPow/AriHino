using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

interface IBooksCharacterModel {

    IDictionary<int, MstBooksCharacterPageData> DisplayCharacterPageDict { get; }
    IObservable<DictionaryAddEvent<int, MstBooksCharacterPageData>> DisplayCharacterPageAddObservable { get; }
    IObservable<DictionaryRemoveEvent<int, MstBooksCharacterPageData>> DisplayCharacterPageRemoveObservable { get; }
}

public class BooksCharacterModel : IBooksCharacterModel
{
    private ReactiveDictionary<int ,MstBooksCharacterPageData> displayCharacterPageDict = new();
    public IDictionary<int, MstBooksCharacterPageData> DisplayCharacterPageDict => displayCharacterPageDict;
    public IObservable<DictionaryAddEvent<int, MstBooksCharacterPageData>> DisplayCharacterPageAddObservable => displayCharacterPageDict.ObserveAdd();
    public IObservable<DictionaryRemoveEvent<int, MstBooksCharacterPageData>> DisplayCharacterPageRemoveObservable => displayCharacterPageDict.ObserveRemove(); 

    public BooksCharacterModel(int[] characterPageIds) {

        SetBooksCharacter(characterPageIds);
    }

    public void SetBooksCharacter(int[] ids) {

        foreach (int id in ids) {

            if(displayCharacterPageDict.ContainsKey(id)) continue;
            var characterPage = MasterDataManager.Instance.GetMasterData<MstBooksCharacterPageData>(id);
            displayCharacterPageDict.Add(id, characterPage);
        }

        foreach(var page in displayCharacterPageDict.Keys) {
            if(!ids.Contains(page)) displayCharacterPageDict.Remove(page);
        }
    }
}
