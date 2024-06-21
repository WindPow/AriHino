using System.Linq;

public class MstBooksCharacterMemoData : MstLanguageData
{
    public int MemoId { get; private set;}
    public int CharaId { get; private set; }
    
    public override void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "memo_id":
                    MemoId = int.Parse(values[i]);
                    break;
                case "chara_id":
                    CharaId = int.Parse(values[i]);
                    break;
                
                // Add more properties if needed
            }
        }
        base.Initialize(headers, values);
    }
}
