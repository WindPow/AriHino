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
}
