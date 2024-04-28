using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public interface IBooksWorldModel {

    IDictionary<int, MstBooksWorldPageData> DisplayWorldPageDict { get; }
    IObservable<DictionaryAddEvent<int, MstBooksWorldPageData>> DisplayWorldPageAddObservable { get; }
    IObservable<DictionaryRemoveEvent<int, MstBooksWorldPageData>> DisplayWorldPageRemoveObservable { get; }

    void SetBooksWorld(int[] ids);
}

public class BooksWorldModel : IBooksWorldModel
{
    private ReactiveDictionary<int ,MstBooksWorldPageData> displayWorldPageDict = new();
    public IDictionary<int, MstBooksWorldPageData> DisplayWorldPageDict => displayWorldPageDict;
    public IObservable<DictionaryAddEvent<int, MstBooksWorldPageData>> DisplayWorldPageAddObservable => displayWorldPageDict.ObserveAdd();
    public IObservable<DictionaryRemoveEvent<int, MstBooksWorldPageData>> DisplayWorldPageRemoveObservable => displayWorldPageDict.ObserveRemove(); 

    public BooksWorldModel(int[] worldPageIds) {

        SetBooksWorld(worldPageIds);
    }

    public void SetBooksWorld(int[] ids) {

        foreach (int id in ids) {

            if(displayWorldPageDict.ContainsKey(id)) continue;
            var characterPage = MasterDataManager.Instance.GetMasterData<MstBooksWorldPageData>(id);
            displayWorldPageDict.Add(id, characterPage);
        }

        foreach(var page in displayWorldPageDict.Keys) {
            if(!ids.Contains(page)) displayWorldPageDict.Remove(page);
        }
    }
}
