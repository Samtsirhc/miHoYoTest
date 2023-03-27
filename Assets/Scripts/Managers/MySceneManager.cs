using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager>
{
    #region 卸载 加载 切换场景
    public AsyncOperation unloadOperation;
    public string unloadSceneName;
    public AsyncOperation loadOperation;
    public string loadSceneName;
    [EditorButton]
    public void UnloadScene(string name, float delay = 0f)
    {
        StartCoroutine(_UnloadScene(name, delay));
    }

    private IEnumerator _UnloadScene(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        unloadSceneName = name;
        Logger.Log($"开始卸载场景 {name}");
        unloadOperation = SceneManager.UnloadSceneAsync(name);
        yield return unloadOperation;
        Logger.Log($"卸载场景完成 {name}");
    }

    [EditorButton]
    public void LoadScene(string name, float delay = 0f)
    {
        StartCoroutine(_LoadScene(name, delay));
    }

    private IEnumerator _LoadScene(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        loadSceneName = name;
        Logger.Log($"开始加载场景 {name}");
        loadOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        yield return loadOperation;
        Logger.Log($"加载场景完成 {name}");

        // 激活加载的场景
        Scene scene = SceneManager.GetSceneByName(name);
        SceneManager.SetActiveScene(scene);
    }
    [EditorButton]
    public void SwitchScene(string from, string to, float delay)
    {
        UnloadScene(from, delay);
        LoadScene(to, delay);
    }
    #endregion
}