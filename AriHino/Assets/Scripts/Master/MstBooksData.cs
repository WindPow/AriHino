using System.Linq;

public class MstBooksData : IMasterData<int> {
    public int ID { get; private set; }
    public int[] CharacterPageIds { get; private set; }
    public int[] WorldPageIds { get; private set; }
    public int[] WardPageIds { get; private set; }
    public int[] CollectPageIds { get; private set; }

    public void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "id":
                    ID = int.Parse(values[i]);
                    break;
                case "character_page_ids":
                    CharacterPageIds = values[i].Split(',').Select(int.Parse).ToArray();
                    break;
                case "world_page_ids":
                    WorldPageIds =values[i].Split(',').Select(int.Parse).ToArray();
                    break;
                case "ward_page_ids":
                    WardPageIds = values[i].Split(',').Select(int.Parse).ToArray();
                    break;
                case "collect_page_ids":
                    CollectPageIds = values[i].Split(',').Select(int.Parse).ToArray();
                    break;
                
                // Add more properties if needed
            }
        }
    }
}