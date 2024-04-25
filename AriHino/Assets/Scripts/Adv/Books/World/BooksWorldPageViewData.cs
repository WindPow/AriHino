

using System.Collections.Generic;
using System.Linq;

public class BooksWorldPageViewData {

    public int ID { get; }
    public string WorldName { get; }
    public string WorldImageFilePath { get; }
    public List<string> ExplanationStrs { get; } = new();

    public BooksWorldPageViewData(MstBooksWorldPageData WorldData) {

        ID = WorldData.ID;
        WorldImageFilePath = WorldData.ImagePath;
        WorldName = MasterDataManager.Instance.GetMasterData<MstWorldData>(WorldData.WorldId).Name;

        // キャラ説明を登録
        var explanationList = MasterDataManager.Instance.GetMasterDataDictionary<MstBooksWorldExplanationData>().Values;
        var createExplanations = explanationList
            .Where(e => e.WorldId == WorldData.WorldId)
            .Where(e => WorldData.ExplanationIds.ToList().Contains(e.ExplanationId));
        foreach (var explanation in createExplanations) {
            ExplanationStrs.Add(explanation.Text);
        }
    }
}