using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public interface IBooksCharacterModel {

    IReadOnlyReactiveDictionary<int, BooksCharacterPageViewData> DisplayCharacterPageDict { get; }

    void SetBooksCharacter(int[] ids);
    void SetBooksCharacter(MstBooksCharacterPageData pageData);
}

public class BooksCharacterModel : IBooksCharacterModel
{
    private ReactiveDictionary<int ,BooksCharacterPageViewData> displayCharacterPageDict = new();
    public IReadOnlyReactiveDictionary<int, BooksCharacterPageViewData> DisplayCharacterPageDict => displayCharacterPageDict;

    public BooksCharacterModel(int[] characterPageIds) {

        SetBooksCharacter(characterPageIds);
    }

    public void SetBooksCharacter(int[] ids) {

        foreach (int id in ids) {

            if(displayCharacterPageDict.ContainsKey(id)) continue;
            var characterPage = MasterDataManager.Instance.GetMasterData<MstBooksCharacterPageData>(id);
            var viewData = new BooksCharacterPageViewData(characterPage);
            displayCharacterPageDict.Add(characterPage.CharaId, viewData);
        }

        // foreach(var page in displayCharacterPageDict.Keys) {
        //     if(!ids.Contains(page)) displayCharacterPageDict.Remove(page);
        // }
    }

    public void SetBooksCharacter(MstBooksCharacterPageData pageData) {
        var viewData = new BooksCharacterPageViewData(pageData);

        if(displayCharacterPageDict.ContainsKey(viewData.CharaId)) {
            displayCharacterPageDict[viewData.CharaId] = viewData;
        }
        else {
            displayCharacterPageDict.Add(viewData.CharaId, viewData);
        }
        
    }
}
