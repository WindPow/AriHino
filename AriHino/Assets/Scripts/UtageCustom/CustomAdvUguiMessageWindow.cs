using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class CustomAdvUguiMessageWindow : AdvUguiMessageWindowTMP
{

	[SerializeField] private GameObject[] activateObjects;

    protected override void LinkIconSub(GameObject icon, bool isActive)
    {
        if (icon == null) return;

			if (!Engine.UiManager.IsShowingMessageWindow)
			{
				icon.SetActive(false);
			}
			else
			{
                
				icon.SetActive(isActive);
			}
    }

	//毎フレームの更新
		protected override void LateUpdate()
		{
			if (Engine.UiManager.Status == AdvUiManager.UiStatus.Default)
			{
				foreach (var obj in activateObjects) {
					obj.SetActive(Engine.UiManager.IsShowingMessageWindow);
				}

				if (Engine.UiManager.IsShowingMessageWindow)
				{
					//ウィンドのアルファ値反映
					if (translateMessageWindowRoot!=null)
					{
						translateMessageWindowRoot.alpha = Engine.Config.MessageWindowAlpha;
					}
				}
			}

			UpdateCurrent();
		}
}
