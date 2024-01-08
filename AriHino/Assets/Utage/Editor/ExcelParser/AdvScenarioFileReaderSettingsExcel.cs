using UnityEngine;

namespace Utage.ExcelParser
{
    //エクセル形式のシナリオリーダーの設定
    [CreateAssetMenu(menuName = "Utage/ScenarioFileReader/Excel", fileName = "ExcelFileReaderSettings")]
    public class AdvScenarioFileReaderSettingsExcel : ScenarioFileReaderSettings, IScenarioFileReaderSettingsExcel
    {
        /// エクセルの数式解析するか
        [SerializeField] bool parseFormula = false;
        public bool ParseFormula
        {
            get => parseFormula;
            set => parseFormula = value;
        }

        /// エクセルの数字解析（桁区切り対策など）
        [SerializeField] bool parseNumeric = false;
        public bool ParseNumeric
        {
            get => parseNumeric;
            set => parseNumeric = value;
        }

        public override IAdvScenarioFileReader CreateReader()
        {
            return new AdvScenarioFileReaderExcel(this);
        }
    }
}
