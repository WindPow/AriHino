// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura


namespace Utage.ExcelParser
{

	//エクセル形式のシナリオリーダー
	public class AdvScenarioFileReaderExcel : IAdvScenarioFileReader
	{
		AdvScenarioFileReaderSettingsExcel Settings { get; }

		public AdvScenarioFileReaderExcel(AdvScenarioFileReaderSettingsExcel settings)
		{
			Settings = settings;
		}
		public bool IsTargetFile(string path)
		{
			if (string.IsNullOrEmpty(path)) return false;
			return ExcelParser.IsExcelFile(path);
		}

		public bool TryReadFile(string path, out StringGridDictionary stringGridDictionary)
		{
			if (!IsTargetFile(path))
			{
				stringGridDictionary = null;
				return false;
			}
			stringGridDictionary = ExcelParser.Read(path, '#', Settings.ParseFormula, Settings.ParseNumeric);
			stringGridDictionary.RemoveSheets(@"^#");
			return true;
		}
	}
}
