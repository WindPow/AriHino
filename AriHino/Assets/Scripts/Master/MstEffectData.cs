public class MstEffectData : IMasterData<int> {
    public int ID { get; private set; }
    public string Name { get; private set; }
    public float LifeTime { get; private set; }
    public string FileName { get; private set; }

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
                    string nameText = values[i].Replace("#", "\n");
                    Name = nameText;
                    break;
                case "life_time":
                    LifeTime = float.Parse(values[i]);
                    break;
                case "file_path":
                    FileName = values[i];
                    break;
                
                // Add more properties if needed
            }
        }
    }

}