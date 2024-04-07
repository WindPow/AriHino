using UnityEngine;
using Cysharp.Threading.Tasks;

public static class AnimationExtensions
{
    public static async UniTask PlayAndWaitForCompletionAsync(this Animation animation)
    {
        animation.Play();

        while (animation.isPlaying)
        {
            await UniTask.Yield();
        }
    }

    public static async UniTask WaitForCompletionAsync(this Animation animation)
    {
        while (animation.isPlaying)
        {
            await UniTask.Yield();
        }
    }
}