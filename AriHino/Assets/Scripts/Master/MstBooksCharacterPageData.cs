using System.Linq;

public class MstBooksCharacterPageData : IMasterData<int>
{
    public int ID { get; private set; }
    public int CharaId { get; private set;}
    public int ProgressNo { get; private set; }
    public int[] ExplanationIds { get; private set; }
    public int[] MemoIds { get; private set;}
    public int[] ImpressionsIds { get; private set;}
    public string ImagePath { get; private set; }

    public void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "id":
                    ID = int.Parse(values[i]);
                    break;
                case "chara_id":
                    CharaId = int.Parse(values[i]);
                    break;
                case "progress_no":
                    ProgressNo = int.Parse(values[i]);
                    break;
                case "explanation_ids":
                    ExplanationIds = values[i].Split('_').Select(int.Parse).ToArray();
                    break;
                case "memo_ids":
                    MemoIds = values[i].Split('_').Select(int.Parse).ToArray();
                    break;
                case "impressions_ids":
                    ImpressionsIds = values[i].Split('_').Select(int.Parse).ToArray();
                    break;
                case "image_path":
                    ImagePath = values[i];
                    break;
                // Add more properties if needed
            }
        }
    }
}
