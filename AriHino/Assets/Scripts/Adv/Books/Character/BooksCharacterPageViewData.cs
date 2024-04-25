

using System.Collections.Generic;
using System.Linq;

public class BooksCharacterPageViewData {

    public int ID { get; }
    public string CharaName { get; }
    public string CharaImageFilePath { get; }
    public List<string> ExplanationStrs { get; } = new();
    public List<string> MemoStrs { get; } = new();
    public List<string> ImpressionsStrs { get; } = new();

    public BooksCharacterPageViewData(MstBooksCharacterPageData characterData) {

        ID = characterData.ID;
        CharaImageFilePath = characterData.ImagePath;
        CharaName = MasterDataManager.Instance.GetMasterData<MstCharacterData>(characterData.CharaId).Name;

        // キャラ説明を登録
        var explanationList = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksCharacterExplanationData>().Values;
        var createExplanations = explanationList
            .Where(e => e.CharaId == characterData.CharaId)
            .Where(e => characterData.ExplanationIds.ToList().Contains(e.ExplanationId));
        foreach (var explanation in createExplanations) {
            ExplanationStrs.Add(explanation.Text);
        }

        // メモを登録
        var memoList = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksCharacterMemoData>().Values;
        var createMemos = memoList
            .Where(e => e.CharaId == characterData.CharaId)
            .Where(e => characterData.MemoIds.ToList().Contains(e.MemoId));
        foreach (var memo in createMemos) {
            MemoStrs.Add(memo.Text);
        }

        // 所感を登録
        var impressionsList = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksCharacterImpressionsData>().Values;
        var createImpressions = impressionsList
            .Where(e => e.CharaId == characterData.CharaId)
            .Where(e => characterData.ImpressionsIds.ToList().Contains(e.ImpressionsId));
        foreach (var impressions in createImpressions) {
            ImpressionsStrs.Add(impressions.Text);
        }
    }
}