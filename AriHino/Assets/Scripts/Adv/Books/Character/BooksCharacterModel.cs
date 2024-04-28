using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public interface IBooksCharacterModel {

    IReadOnlyReactiveDictionary<int, MstBooksCharacterPageData> DisplayCharacterPageDict { get; }

    void SetBooksCharacter(int[] ids);
    void UpdateBooksCharacter(MstBooksCharacterPageData pageData);
}

public class BooksCharacterModel : IBooksCharacterModel
{
    private ReactiveDictionary<int ,MstBooksCharacterPageData> displayCharacterPageDict = new();
    public IReadOnlyReactiveDictionary<int, MstBooksCharacterPageData> DisplayCharacterPageDict => displayCharacterPageDict;

    public BooksCharacterModel(int[] characterPageIds) {

        SetBooksCharacter(characterPageIds);
    }

    public void SetBooksCharacter(int[] ids) {

        foreach (int id in ids) {

            if(displayCharacterPageDict.ContainsKey(id)) continue;
            var characterPage = MasterDataManager.Instance.GetMasterData<MstBooksCharacterPageData>(id);
            displayCharacterPageDict.Add(characterPage.CharaId, characterPage);
        }

        // foreach(var page in displayCharacterPageDict.Keys) {
        //     if(!ids.Contains(page)) displayCharacterPageDict.Remove(page);
        // }
    }

    public void UpdateBooksCharacter(MstBooksCharacterPageData pageData) {
        
        displayCharacterPageDict[pageData.CharaId] = pageData;
        
    }
}
