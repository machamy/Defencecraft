using System;
using System.Collections;
using _02.Scirpts.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>{
    
    public void MoveScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ClickTest()
    {
        Debug.Log("Test");
    }


}