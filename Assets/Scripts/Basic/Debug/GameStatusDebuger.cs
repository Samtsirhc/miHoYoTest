using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameStatusDebuger : MonoBehaviour
{
    public Text textObj_01;
    public string text_01;
    private void Refresh()
    {
        text_01 = "";
        if (MySceneManager.Instance.unloadOperation?.isDone == false)
        {
            text_01 += $"正在卸载场景 {MySceneManager.Instance.unloadSceneName} {MySceneManager.Instance.unloadOperation.progress} \n";
        }
        if (MySceneManager.Instance.loadOperation?.isDone == false)
        {
            text_01 += $"正在加载场景 {MySceneManager.Instance.loadSceneName} {MySceneManager.Instance.loadOperation.progress} \n";
        }
        textObj_01.text = text_01;
    }
    private void FixedUpdate()
    {
        Refresh();
    }
}
