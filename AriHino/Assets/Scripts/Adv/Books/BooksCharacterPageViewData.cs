

using System.Collections.Generic;

public class BooksCharacterPageViewData {

    public int ID { get; }
    public string CharaName { get; }
    public string CharaImageFilePath { get; }
    public List<string> ExplanationStrs { get; }
    public List<string> MemoStrs { get; }
    public List<string> ImpressionsStrs { get; }

    public BooksCharacterPageViewData(MstBooksCharacterPageData characterData) {

        ID = characterData.ID;
        CharaImageFilePath = characterData.ImageFilePath;
        CharaName = MasterDataManager.Instance.GetMasterData<MstCharacterData>(characterData.ID).Name;

        // キャラ説明を登録
        foreach (var explanationId in characterData.ExplanationIds) {
            var explanation = MasterDataManager.Instance.GetMasterData<MstBooksCharacterExplanationData>(explanationId);
            ExplanationStrs.Add(explanation.Text);
        }

        // メモを登録
        foreach (var memoId in characterData.MemoIds) {
            var memo = MasterDataManager.Instance.GetMasterData<MstBooksCharacterMemoData>(memoId);
            MemoStrs.Add(memo.Text);
        }

        // 所感を登録
        foreach (var impressionsId in characterData.ImpressionsIds) {
            var impressions = MasterDataManager.Instance.GetMasterData<MstBooksCharacterImpressionsData>(impressionsId);
            ImpressionsStrs.Add(impressions.Text);
        }
    }
}