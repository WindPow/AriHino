using UnityEngine;
using Utage;
using UtageExtensions;

public class TitleSceneController : UtageUguiTitle
{
    protected override void OnOpen(){

        SceneLoadManager.LoadScene(AppDefine.SceneName.MainGame);
    }
}