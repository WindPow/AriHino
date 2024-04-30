using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public interface IBooksCollectModel {

    IReadOnlyDictionary<int, BooksCollectContentsViewData> DisplayCollectPageDict { get; }

    void SetBooksCollect(List<MstCollectItemData> list);
    void OpenBooksCollect(MstCollectItemData data);

}

public class BooksCollectModel : IBooksCollectModel
{
    private Dictionary<int ,BooksCollectContentsViewData> displayCollectPageDict = new();
    public IReadOnlyDictionary<int, BooksCollectContentsViewData> DisplayCollectPageDict => displayCollectPageDict;

    public BooksCollectModel() {

        var collectList = MasterDataManager.Instance.GetMasterDataDictionary<MstCollectItemData>().Values.ToList();
        SetBooksCollect(collectList);
    }

    public void SetBooksCollect(List<MstCollectItemData> list) {

        foreach (var collect in list) {

            if(displayCollectPageDict.ContainsKey(collect.ID)) continue;
            var collectViewData = new BooksCollectContentsViewData(collect);
            displayCollectPageDict.Add(collect.ID, collectViewData);
        }
    }

    public void OpenBooksCollect(MstCollectItemData data) {

        displayCollectPageDict[data.ID].SetOpen(true);
    }
}
