using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utage {

    public class CustomCommand : AdvCustomCommandManager {

        public override void OnBootInit()
        {
            Utage.AdvCommandParser.OnCreateCustomCommandFromID+= CreateCustomCommand;
        }
 
        //AdvEnginのクリア処理のときに呼ばれる
        public override void OnClear()
        {
        }
         
        //カスタムコマンドの作成用コールバック
        public void CreateCustomCommand(string id, StringGridRow row, AdvSettingDataManager dataManager, ref AdvCommand command )
        {
            switch (id)
            {
                case "CollectItem":
                    command = new AdvCommandCollectItem(row);
                    break;
            }
        }
    }
}


