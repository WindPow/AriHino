using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utage {

public class AdvCommandCollectItem : AdvCommand
{
        private int collectStateId = 0;

        public AdvCommandCollectItem(StringGridRow row) : base(row) {

            collectStateId = ParseCell<int>(AdvColumnName.Arg1);
        }

        public override void DoCommand(AdvEngine engine)
        {
            
            AdvCollectItemManager.Instance.SetCollectItem(collectStateId);
        }
    
}

}


