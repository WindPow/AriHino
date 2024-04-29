using System;
using UniRx;

public class BooksCollectContentsViewData {

    public MstCollectItemData MstCollectItemData { get; }
    public bool IsOpen { get; private set; }

    public IObservable<bool> SetOpenObservable => setOpenSubject;
    private Subject<bool> setOpenSubject = new();

    public BooksCollectContentsViewData(MstCollectItemData mstCollectItemData) {

        MstCollectItemData = mstCollectItemData;
        IsOpen = mstCollectItemData.OpenedFlg;
    }

    public void SetOpen(bool isOpen) {
        IsOpen = isOpen;
        setOpenSubject.OnNext(IsOpen);
    }
}