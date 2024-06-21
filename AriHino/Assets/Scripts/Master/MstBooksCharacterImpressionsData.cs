using System.Linq;

public class MstBooksCharacterImpressionsData : MstLanguageData
{
    public int ImpressionsId { get; private set;}
    public int CharaId { get; private set; }
    
    public override void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "impressions_id":
                    ImpressionsId = int.Parse(values[i]);
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
