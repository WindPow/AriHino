using System.Linq;

public class MstBooksCharacterMemoData : IMasterData<int>
{
    public int ID { get; private set; }
    public int MemoId { get; private set;}
    public int CharaId { get; private set; }
    public string Text { get; private set; }
    
    public void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "id":
                    ID = int.Parse(values[i]);
                    break;
                case "memo_id":
                    MemoId = int.Parse(values[i]);
                    break;
                case "chara_id":
                    CharaId = int.Parse(values[i]);
                    break;
                case "text":
                    string text = values[i].Replace("#", "\n");
                    Text = text;
                    break;
                
                // Add more properties if needed
            }
        }
    }
}
