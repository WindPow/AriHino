public class MstAdvCollectItem : IMasterData<int>
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Type { get; private set;}

    public void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "id":
                    ID = int.Parse(values[i]);
                    break;
                case "name":
                    Name = values[i];
                    break;
                case "description":
                    Description = values[i];
                    break;
                case "type":
                    Type = int.Parse(values[i]);
                    break;
                // Add more properties if needed
            }
        }
    }

    public int GetKey()
    {
        return ID;
    }
}
