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
    void RemoveBooksCharacter(int characterId);
}

public class BooksCharacterModel : IBooksCharacterModel
{
    private ReactiveDictionary<int ,BooksCharacterPageViewData> displayCharacterPageDict = new();
    public IReadOnlyReactiveDictionary<int, BooksCharacterPageViewData> DisplayCharacterPageDict => displayCharacterPageDict;

    public BooksCharacterModel(int[] characterPageIds) {

        SetBooksCharacter(characterPageIds);
    }

    public void SetBooksCharacter(int[] ids) {

        displayCharacterPageDict.Clear();

        foreach (int id in ids) {

            if(displayCharacterPageDict.ContainsKey(id)) continue;
            var characterPage = MasterDataManager.Instance.GetMasterData<MstBooksCharacterPageData>(id);
            var viewData = new BooksCharacterPageViewData(characterPage);
            displayCharacterPageDict.Add(characterPage.CharaId, viewData);
        }
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

    public void RemoveBooksCharacter(int characterId) {

        if(displayCharacterPageDict.ContainsKey(characterId)) {
            displayCharacterPageDict.Remove(characterId);
        }
    }
}
