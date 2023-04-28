using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンマネージャー
/// </summary>
public class SceneLoadManager
{
    /// <summary>
    /// シーンをロード
    /// </summary>
    /// <param name="sceneIndex"></param>
    public static void LoadScene(AppDefine.SceneName sceneName)
    {
        SceneManager.LoadScene((int)sceneName);
    }

    /// <summary>
    /// シーンを追加
    /// </summary>
    /// <param name="sceneIndex"></param>
    public static void AddScene(AppDefine.SceneName sceneName)
    {
        SceneManager.LoadScene((int)sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// シーンをアンロード
    /// </summary>
    /// <param name="sceneIndex"></param>
    public static void UnloadScene(AppDefine.SceneName sceneName)
    {
        SceneManager.UnloadSceneAsync((int)sceneName);
    }

    /// <summary>
    /// シーンを再読み込み
    /// </summary>
    public static void ReloadCurrentScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public static void QuitGame()
    {
        Application.Quit();
    }
}