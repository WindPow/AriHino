using System.Linq;

public class MstBooksWorldExplanationData : IMasterData<int>
{
    public int ID { get; private set; }
    public int ExplanationId { get; private set;}
    public int WorldId { get; private set; }
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
                case "world_id":
                    WorldId = int.Parse(values[i]);
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
