public class MstCollectItemData : MstLanguageData
{
    public int Type { get; private set;}
    public bool OpenedFlg { get; private set;}
    public string ImagePath { get; private set;}

    public override void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "type":
                    Type = int.Parse(values[i]);
                    break;
                case "opened_flg":
                    OpenedFlg = int.Parse(values[i]) != 0;
                    break;
                case "image_path":
                    ImagePath = values[i];
                    break;
                // Add more properties if needed
            }
        }
        base.Initialize(headers, values);
    }
}
