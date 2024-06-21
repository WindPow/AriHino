using System.Linq;

public class MstBooksWorldExplanationData : MstLanguageData
{
    public int ExplanationId { get; private set;}
    public int WorldId { get; private set; }
    
    public override void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "explanation_id":
                    ExplanationId = int.Parse(values[i]);
                    break;
                case "world_id":
                    WorldId = int.Parse(values[i]);
                    break;
                
                // Add more properties if needed
            }
        }
        base.Initialize(headers, values);
    }
}
