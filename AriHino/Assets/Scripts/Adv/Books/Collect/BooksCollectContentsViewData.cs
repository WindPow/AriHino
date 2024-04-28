using System;
using UniRx;

public class BooksCollectContentsViewData {

    public MstCollectItemData MstCollectItemData { get; }
    public bool IsOpened { get; private set; }

    public IObservable<bool> SetOpenObservable => setOpenSubject;
    private Subject<bool> setOpenSubject = new();

    public BooksCollectContentsViewData(MstCollectItemData mstCollectItemData) {

        MstCollectItemData = mstCollectItemData;
        IsOpened = mstCollectItemData.OpenedFlg;
    }

    public void SetOpen(bool isOpen) {
        IsOpened = isOpen;
        setOpenSubject.OnNext(IsOpened);
    }
}