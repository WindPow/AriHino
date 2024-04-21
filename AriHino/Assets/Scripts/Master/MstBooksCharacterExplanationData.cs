using System.Linq;

public class MstBooksCharacterExplanationData : IMasterData<int>
{
    public int ID { get; private set; }
    public int ExplanationId { get; private set;}
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
                case "explanation_id":
                    ExplanationId = int.Parse(values[i]);
                    break;
                case "chara_id":
                    CharaId = int.Parse(values[i]);
                    break;
                case "text":
                    Text = values[i];
                    break;
                
                // Add more properties if needed
            }
        }
    }
}
