using System;
using System.Collections.Generic;
using _02.Scirpts.Audio;
using _02.Scirpts.Dictionary;
using _02.Scirpts.Ingame.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{

    private Stack<BaseUI> _uiStack = new();
    
    [SerializeField]
    private SerializableDict<UIPrefabType, GameObject> _prefabDict;

    private Dictionary<UIPrefabType, GameObject> _uiGameObjectDict;

    [Header("Sounds")]
    [SerializeField] private AudioChannelSO sfxSoundChannel;
    [SerializeField] private AudioConfigurationSO sfxConfig;
    [SerializeField] private AudioQueueSO ButtonClickSound;

    public void Awake()
    {
        _uiGameObjectDict = new();
        
        foreach (UIPrefabType prefabType in Enum.GetValues(typeof(UIPrefabType)))
        {
            if (prefabType == UIPrefabType.None)
                continue;
            GameObject prefab = _prefabDict[prefabType];
            _uiGameObjectDict[prefabType] = prefab.GetComponent<BaseUI>().getInstance();
            _uiGameObjectDict[prefabType].SetActive(false);
        }
        
        
        AddUI(UIPrefabType.UI_MainMenu);
    }


    
    public void MoveScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
    // 시작 화면
    public void ClickStart()
    {
        MoveScene("MapScene");
        RemoveAllUI();
        
        Instance.AddUI(UIPrefabType.UI_Map);
    }
    
    public void OpenSettingUI()
    {
        PlayClickSound();
        Instance.AddUI(UIPrefabType.UI_Setting);
    }

    public void OpenCreditUI()
    { 
        Instance.AddUI(UIPrefabType.UI_Credit);
    }

    public void OpenInintializer()
    {
        Instance.AddUI(UIPrefabType.UI_InitConfirm);
    }

    public void ClickFocusContainer()
    {
        Instance.RemoveUI();
    }

    public void ExitUI()
    {
        Instance.RemoveUI();
    }
    
    
    // 맵 화면

    public void ClickDifficulty()
    {
        Instance.AddUI(UIPrefabType.UI_DifficultySelect);
    }

    public void SwapDifficulty(int i)
    {
        // TODO : 난이도 설정 
    }

    public void EnterMap()
    {
        MoveScene("GameScene");
        Instance.RemoveUI();
    }
    
    // 게임 화면

    public void ClickOption()
    {
        Instance.AddUI(UIPrefabType.UI_Setting);
        
    }


    public void PlayClickSound()
    {
        sfxSoundChannel.RaisePlayEvent(ButtonClickSound,sfxConfig);
    }

    public void AddUI(UIPrefabType uiPrefabType)
    {

        GameObject goUI = _uiGameObjectDict[uiPrefabType];
        
        if (uiPrefabType != UIPrefabType.UI_MainMenu)
        {
            _uiGameObjectDict[UIPrefabType.UI_FocusContainer].SetActive(true);
        }

        goUI.SetActive(true);
        
        _uiStack.Push(goUI.GetComponent<BaseUI>());
    }

    public void RemoveUI()
    {
        BaseUI baseUI = _uiStack.Pop();

        bool needFocusRemoval = true;
        foreach (BaseUI remainBaseUI in _uiStack)
        {
            if (remainBaseUI.IsFocused)
            {
                needFocusRemoval = false;
                break;
            }
        }

        if (needFocusRemoval)
        {
            _uiGameObjectDict[UIPrefabType.UI_FocusContainer].SetActive(false);
        }

        baseUI.gameObject.SetActive(false);
    }

    public void RemoveAllUI()
    {
        while (_uiStack.Count > 0)
        {
            RemoveUI();
        }
    }
    

    public enum UIPrefabType
    {
        None,
        UI_MainMenu,
        UI_Map,
        UI_Campaign,
        UI_FocusContainer,
        UI_Credit,
        UI_DifficultySelect,
        UI_Setting,
        UI_InitConfirm,
        UI_Confirm,
        UI_GameOver
    }
    
    


}