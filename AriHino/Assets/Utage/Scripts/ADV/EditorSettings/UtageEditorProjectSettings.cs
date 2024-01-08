#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utage
{
    //宴のプロジェクトで共有するエディター設定
    //プロジェクトのProjectSettingsフォルダ以下に置く
    [FilePath("ProjectSettings/UtageEditorProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class UtageEditorProjectSettings : EditorSettingsSingleton<UtageEditorProjectSettings>
    {
        [Serializable]
        public class ImportSettings
        {

            /// 末尾に指定の数以上の空白行があったら警告を出す
            [SerializeField] int checkBlankRowCount = 1000;
            public int CheckBlankRowCount => checkBlankRowCount;

            /// 指定の数以上の列数があった場合に警告を出す
            [SerializeField] int checkColumnCount = 500;
            public int CheckCellCount => checkColumnCount;

            /// インポート時にWaitTypeをチェックするか
            [SerializeField] bool checkWaitType = false;
            public bool CheckWaitType => checkWaitType;

            /// インポート時にセルの終端の空白をチェックする
            [SerializeField] bool checkWhiteSpaceEndOfCell = true;
            public bool CheckWhiteSpaceEndOfCell => checkWhiteSpaceEndOfCell;
        }
        [SerializeField, UnfoldedSerializable] ImportSettings importSettings = new();
        public ImportSettings ImportSetting => importSettings;
    }
}
#endif
