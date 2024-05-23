using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject go_ingameParent;
    [SerializeField] private GameObject go_pauseParent;

    /// <summary>
    ///  OnGamePaused Subscribe - UI 숨길 거 숨기기
    /// </summary>
    /// <param name="paused"></param>
    public void OnPaused()
    {
        go_ingameParent.SetActive(true);
        go_pauseParent.SetActive(false);
    }
    
    
    /// <summary>
    /// OnGameResumed Subscribe - UI 보일 거 보이게
    /// </summary>
    public void onResumed()
    {
        go_ingameParent.SetActive(false);
        go_pauseParent.SetActive(true);
    }

}