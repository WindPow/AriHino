using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public interface IBooksCollectModel {

    IDictionary<int, BooksCollectContentsViewData> DisplayCollectPageDict { get; }
    IObservable<DictionaryAddEvent<int, BooksCollectContentsViewData>> DisplayCollectPageAddObservable { get; }
    IObservable<DictionaryRemoveEvent<int, BooksCollectContentsViewData>> DisplayCollectPageRemoveObservable { get; }

    void SetBooksCollect(List<MstCollectItemData> list);
    void OpenBooksCollect(MstCollectItemData data);

}

public class BooksCollectModel : IBooksCollectModel
{
    private ReactiveDictionary<int ,BooksCollectContentsViewData> displayCollectPageDict = new();
    public IDictionary<int, BooksCollectContentsViewData> DisplayCollectPageDict => displayCollectPageDict;
    public IObservable<DictionaryAddEvent<int, BooksCollectContentsViewData>> DisplayCollectPageAddObservable => displayCollectPageDict.ObserveAdd();
    public IObservable<DictionaryRemoveEvent<int, BooksCollectContentsViewData>> DisplayCollectPageRemoveObservable => displayCollectPageDict.ObserveRemove(); 

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
