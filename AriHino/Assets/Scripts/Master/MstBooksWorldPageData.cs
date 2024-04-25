using System.Linq;

public class MstBooksWorldPageData : IMasterData<int>
{
    public int ID { get; private set; }
    public int WorldId { get; private set; }
    public int[] ExplanationIds { get; private set; }
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
                case "world_id":
                    WorldId = int.Parse(values[i]);
                    break;
                case "explanation_ids":
                    ExplanationIds = values[i].Split(',').Select(int.Parse).ToArray();
                    break;
                case "image_path":
                    ImagePath = values[i];
                    break;
                // Add more properties if needed
            }
        }
    }
}
