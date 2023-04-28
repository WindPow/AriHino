using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

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

    [SerializeField] private CanvasGroup fadeWhite;

    [Range(0f, 1f)] public float fadeWhiteIntensity;

    [SerializeField] private CanvasGroup fadeBlack;

    [Range(0f, 1f)] public float fadeBlackIntensity;

    public static ScreenEffectManager GetInstance()
    {
        return instance;
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

        fadeWhite = fadeWhite ?? GameObject.Find("FadeWhite").GetComponent<CanvasGroup>();
        fadeBlack = fadeBlack ?? GameObject.Find("FadeBlack").GetComponent<CanvasGroup>();
    }

    // private void Update()
    // {
    //     // エフェクトの強度を設定する
    //     SetEffectIntensity(effectBlurIntensity);

    //     SetFadeWhiteIntensity(fadeWhiteIntensity);

    //     SetFadeBlackIntensity(fadeBlackIntensity);
    // }

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

    // 白フェードの強度を調整
    public void SetFadeWhiteIntensity(float intensity)
    {
        if(!fadeWhite) return;

        fadeWhite.alpha = intensity;
    }

    // 黒フェードの強度を調整
    public void SetFadeBlackIntensity(float intensity)
    {
        if(!fadeBlack) return;

        fadeBlack.alpha = intensity;
    }

    /// <summary>
    /// 白フェードアウトする（コールバック設定可能）
    /// </summary>
    /// <param name="endCallback"></param>
    public void FadeOutWhiteSequence(float duration, UnityAction endCallback = null){

        fadeWhite.gameObject.SetActive(true);
        fadeWhiteIntensity = 0;

        fadeWhite.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => endCallback?.Invoke());
    }

    /// <summary>
    /// 白フェードインする（コールバック設定可能）
    /// </summary>
    /// <param name="endCallback"></param>
    public void FadeInWhiteSequence(float duration, UnityAction endCallback = null){
        fadeWhiteIntensity = 1f;

        fadeWhite.DOFade(0f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                fadeWhite.gameObject.SetActive(false);
                endCallback?.Invoke();
            }
        );
    }

    /// <summary>
    /// 黒フェードアウトする（コールバック設定可能）
    /// </summary>
    /// <param name="endCallback"></param>
    public void FadeOutBlackSequence(float duration, UnityAction endCallback = null){
        
        fadeBlack.gameObject.SetActive(true);
        fadeBlackIntensity = 0;

        fadeBlack.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => endCallback?.Invoke());
    }

    /// <summary>
    /// 黒フェードインする（コールバック設定可能）
    /// </summary>
    /// <param name="endCallback"></param>
    public void FadeInBlackSequence(float duration, UnityAction endCallback = null){
        
        fadeBlackIntensity = 1f;

        fadeBlack.DOFade(0f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                fadeBlack.gameObject.SetActive(false);
                endCallback?.Invoke();
            }
        );
    }

    public void ResetFade(){
        fadeBlackIntensity = 0;
        fadeWhiteIntensity = 0;
        postProcessVolume.weight = 0;
    }

    public void ResetFadeBlack(){

        fadeBlackIntensity = 0;
    }
}
