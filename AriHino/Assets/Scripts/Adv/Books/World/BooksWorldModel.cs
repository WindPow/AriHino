using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public interface IBooksWorldModel {

    IReadOnlyReactiveDictionary<int, BooksWorldPageViewData> DisplayWorldPageDict { get; }
    
    void SetBooksWorld(int[] ids);
    void SetBooksWorld(MstBooksWorldPageData pageData);
}

public class BooksWorldModel : IBooksWorldModel
{
    private ReactiveDictionary<int ,BooksWorldPageViewData> displayWorldPageDict = new();
    public IReadOnlyReactiveDictionary<int, BooksWorldPageViewData> DisplayWorldPageDict => displayWorldPageDict;

    public BooksWorldModel(int[] worldPageIds) {

        SetBooksWorld(worldPageIds);
    }

    public void SetBooksWorld(int[] ids) {

        foreach (int id in ids) {

            if(displayWorldPageDict.ContainsKey(id)) continue;
            var characterPage = MasterDataManager.Instance.GetMasterData<MstBooksWorldPageData>(id);
            var viewData = new BooksWorldPageViewData(characterPage);
            displayWorldPageDict.Add(id, viewData);
        }

        foreach(var page in displayWorldPageDict.Keys) {
            if(!ids.Contains(page)) displayWorldPageDict.Remove(page);
        }
    }

    public void SetBooksWorld(MstBooksWorldPageData pageData) {
        var viewData = new BooksWorldPageViewData(pageData);

        if(displayWorldPageDict.ContainsKey(pageData.WorldId)) {
            displayWorldPageDict[viewData.WorldId] = viewData;
        }
        else {
            displayWorldPageDict.Add(viewData.WorldId, viewData);
        }
        
    }
}
