using System;
using UnityEngine;

namespace Utage
{
    //ポストエフェクトをRenderPipelineによって処理を切り替えて実行するためのインターフェース
    public interface IAdvPostEffectRenderPipelineBridge
    {
        (IPostEffectStrength effect, float start, float end) DoCommandColorFade(Camera targetCamera, IAdvCommandFade command);
        (IPostEffectStrength effect, float start, float end) DoCommandRuleFade(Camera targetCamera, IAdvCommandFade command);
        (IPostEffect effect, Action onComplete) DoCommandImageEffect(Camera targetCamera, IAdvCommandImageEffect command,  Action onComplete);
        void DoCommandImageEffectAllOff(Camera targetCamera, IAdvCommandImageEffect command, Action onComplete);
    }
}
