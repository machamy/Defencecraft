using System;
using System.Collections.Generic;
using _02.Scirpts.Dictionary;
using _02.Scirpts.Ingame.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    private Stack<BaseUI> _uiStack = new();
    
    [SerializeField]
    private SerializableDict<UIPrefabType, GameObject> _prefabDict;

    private Dictionary<UIPrefabType, GameObject> _uiGameObjectDict;

    public void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

        _uiGameObjectDict = new();
        
        foreach (UIPrefabType prefabType in Enum.GetValues(typeof(UIPrefabType)))
        {
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
        instance.removeUI();
    }
    
    public void OpenSettingUI()
    {
        instance.AddUI(UIPrefabType.UI_Setting);
    }

    public void OpenCreditUI()
    { 
        instance.AddUI(UIPrefabType.UI_Credit);
    }

    public void ClickFocusContainer()
    {
        instance.removeUI();
    }
    
    
    // 맵 화면

    public void ClickDifficulty()
    {
        instance.AddUI(UIPrefabType.UI_DifficultySelect);
    }

    public void SwapDifficulty(int i)
    {
        // TODO : 난이도 설정 
    }

    public void EnterMap()
    {
        MoveScene("GameScene");
        instance.removeUI();
    }
    
    // 게임 화면

    public void ClickOption()
    {
        instance.AddUI(UIPrefabType.UI_Setting);
        
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

    public void removeUI()
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

    public enum UIPrefabType
    {
        None,
        UI_MainMenu,
        UI_FocusContainer,
        UI_Credit,
        UI_DifficultySelect,
        UI_Setting,
        UI_Confirm,
        UI_Map,
        UI_Campaign,
        UI_GameOver
    }
    
    


}