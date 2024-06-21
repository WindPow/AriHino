using Utage;

public class MstLanguageData : IMasterData<int>
{
    public int ID { get; private set; }
    private string name;
    public string Name { get => SelectLanguageName(); }
    protected string EnglishName { get; private set; }
    private string text;
    public string Text { get => SelectLanguageText(); }
    protected string EnglishText { get; private set; }
    
    public virtual void Initialize(string[] headers, string[] values)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            switch (headers[i].ToLower())
            {
                case "id":
                    ID = int.Parse(values[i]);
                    break;
                case "name":
                    string name = values[i].Replace("#", "\n");
                    this.name = name;
                    break;
                case "english_name":
                    string englishName = values[i].Replace("#", "\n");
                    EnglishName = englishName;
                    break;
                case "text":
                    string text = values[i].Replace("#", "\n");
                    this.text = text;
                    break;
                case "english_text":
                    string englishText = values[i].Replace("#", "\n");
                    EnglishText = englishText;
                    break;
                
                // Add more properties if needed
            }
        }
    }

    private string SelectLanguageName() {
        return LanguageManager.Instance.CurrentLanguage switch
        {
            "Japanese" => name,
            "English" => EnglishName,
            _ => null,
        };
    }

    private string SelectLanguageText() {
        return LanguageManager.Instance.CurrentLanguage switch
        {
            "Japanese" => text,
            "English" => EnglishText,
            _ => null,
        };
    }
}
