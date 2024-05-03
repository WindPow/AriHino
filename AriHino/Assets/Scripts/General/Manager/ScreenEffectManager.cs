using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using UniRx;
using Cysharp.Threading.Tasks;

public class ScreenEffectManager : MonoBehaviour
{
    // シングルトンインスタンス
    private static ScreenEffectManager instance;

    [Header("ブラー")]

    // エフェクトに使用するカメラ
    [SerializeField] private Camera effectCamera;

    // カメラにアタッチされているPostProcessVolumeコンポーネント
    private PostProcessVolume postProcessVolume;

    // エフェクトの強度
    [Range(0f, 1f)] public float effectBlurIntensity;

    [Header("フェード")]

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Image fadeImage;

    public static ScreenEffectManager Instance {

        get {
            if (instance == null) instance = GameObject.FindObjectOfType<ScreenEffectManager>();

            if (instance == null) {
                GameObject singletonObject = new GameObject(typeof(ScreenEffectManager).Name);
                instance = singletonObject.AddComponent<ScreenEffectManager>();
            }
            return instance;
        }
    }
    

    private void Awake()
    {
        // シングルトンインスタンスを設定する
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // エフェクトに使用するカメラからPostProcessVolumeコンポーネントを取得する
        postProcessVolume = effectCamera.GetComponentInChildren<PostProcessVolume>();
        //SetEffectIntensity(0);

        fadeCanvasGroup = fadeCanvasGroup ?? GameObject.Find("FadeWhite").GetComponent<CanvasGroup>();
        SetFadeIntensity(0);
    }

    // エフェクトの強度を設定するメソッド
    public void SetEffectIntensity(float intensity)
    {
        // PostProcessVolumeのDepthOfFieldエフェクトにアクセスして、FocusDistanceパラメーターの値を変更する
        if (postProcessVolume.profile.TryGetSettings(out DepthOfField depthOfField))
        {
            depthOfField.focusDistance.value = Mathf.Lerp(30f, 0.1f, intensity);
        }
    }

    /// <summary>
    /// ブラーアウトを再生する（コールバック設定可能）
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endCallback"></param>
    public void BlurOutSequence(float duration, UnityAction endCallback = null)
    {
        if(postProcessVolume == null) return;

        postProcessVolume.weight = 0;

        DOTween.To(() => postProcessVolume.weight, x => postProcessVolume.weight = x, 1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => endCallback?.Invoke());;
    }

    /// <summary>
    /// ブラーインを再生する（コールバック設定可能）
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endCallback"></param>
    public void BlurInSequence(float duration, UnityAction endCallback = null)
    {
        if(postProcessVolume == null) return;

        postProcessVolume.weight = 1;

        DOTween.To(() => postProcessVolume.weight, x => postProcessVolume.weight = x, 0, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => endCallback?.Invoke());
    }

    // フェードの強度を調整
    public void SetFadeIntensity(float intensity)
    {
        if(!fadeCanvasGroup) return;

        fadeCanvasGroup.alpha = intensity;
    }

    /// <summary>
    /// フェードアウトする（コールバック設定可能）
    /// </summary>
    /// <param name="endCallback"></param>
    public void FadeOutSequence(float duration, Color color, UnityAction endCallback = null){

        fadeImage.color = color;
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0;

        fadeCanvasGroup.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => endCallback?.Invoke());
    }

    /// <summary>
    /// フェードインする（コールバック設定可能）
    /// </summary>
    /// <param name="endCallback"></param>
    public void FadeInSequence(float duration, UnityAction endCallback = null){
        
        fadeCanvasGroup.alpha = 1;

        fadeCanvasGroup.DOFade(0f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                fadeCanvasGroup.gameObject.SetActive(false);
                endCallback?.Invoke();
            }
        );
    }


    public void ResetFade(){
        fadeCanvasGroup.alpha = 0;
        postProcessVolume.weight = 0;
    }

}
