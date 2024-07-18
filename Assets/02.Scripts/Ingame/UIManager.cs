using System;
using System.Collections.Generic;
using _02.Scirpts.Audio;
using _02.Scirpts.Dictionary;
using _02.Scirpts.Ingame;
using _02.Scirpts.Ingame.Entity;
using _02.Scirpts.Ingame.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{

    private Stack<BaseUI> _uiStack = new();
    
    [SerializeField]
    private SerializableDict<UIPrefabType, GameObject> _uiGameObjectDict;

    [Header("Sounds")]
    [SerializeField] private AudioChannelSO sfxSoundChannel;
    [SerializeField] private AudioConfigurationSO sfxConfig;
    [SerializeField] private AudioQueueSO ButtonClickSound;


    public void MoveScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #region Global

    public void ClickFocusContainer()
    {
        BaseUI _nowUI = Instance._uiStack.Peek();
        if(_nowUI.RemoveUIFocusClicked)
            Instance.RemoveUI();
    }
    
    public void ExitUI()
    {
        Instance.RemoveUI();
    }
    
    #endregion
    
    
    #region MainMenu
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
        _uiGameObjectDict[UIPrefabType.UI_Setting].GetComponent<SettingUI>().setGameQuitButtonVisibility(false);
    }

    public void OpenCreditUI()
    { 
        Instance.AddUI(UIPrefabType.UI_Credit);
    }

    public void OpenInintializer()
    {
        Instance.AddUI(UIPrefabType.UI_InitConfirm);
    }
    #endregion
    
    
    #region Map
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
    #endregion


    #region Ingame

    public void PauseClicked()
    {
        GameManager.Instance.Pause(true);
    }
    
    public void PlayClicked()
    {
        GameManager.Instance.Pause(false);
    }

    public void SettingIngameClicked()
    {
        Instance.AddUI(UIPrefabType.UI_Setting);
        Instance._uiGameObjectDict[UIPrefabType.UI_Setting].GetComponent<SettingUI>().setGameQuitButtonVisibility(true);
    }

    public void OpenGameQuitConfirm()
    {
        Instance.AddUI(UIPrefabType.UI_Confirm);
    }

    public void QuitGame()
    {
        Instance.MoveScene("MapScene");
    }

    #endregion

    public void PlayClickSound()
    {
        sfxSoundChannel.RaisePlayEvent(ButtonClickSound,sfxConfig);
    }

    public void AddUI(UIPrefabType uiPrefabType)
    {

        GameObject goUI = _uiGameObjectDict[uiPrefabType];
        BaseUI baseUI = goUI.GetComponent<BaseUI>();
        
        if (baseUI.IsFocused)
        {
            GameObject goFocus = _uiGameObjectDict[UIPrefabType.UI_FocusContainer];
            goFocus.SetActive(true);
            
            moveFocusContainerForwardTo(goUI);
        }

        goUI.SetActive(true);
        
        _uiStack.Push(goUI.GetComponent<BaseUI>());
    }

    public void RemoveUI()
    {

        if (_uiStack.Count == 0)
            return;
        
        BaseUI baseUI = _uiStack.Pop();

        bool needFocusRemoval = true;

        foreach (BaseUI remainBaseUI in _uiStack)
        {
            if (remainBaseUI.IsFocused)
            {
                needFocusRemoval = false;
                moveFocusContainerForwardTo(remainBaseUI.GameObject());
                break;
            }
        }

        if (needFocusRemoval)
        {
            _uiGameObjectDict[UIPrefabType.UI_FocusContainer].SetActive(false);
            moveFocusContainerToFirst();
        }

        baseUI.gameObject.SetActive(false);
    }

    public GameObject GetUI(UIPrefabType type)
    {
        return _uiGameObjectDict[type];
    }

    public void RemoveAllUI()
    {
        while (_uiStack.Count > 0)
        {
            BaseUI baseUI = _uiStack.Pop();
            baseUI.gameObject.SetActive(false);
        }
    }


    private void moveFocusContainerForwardTo(GameObject goUI)
    {
        GameObject goFocus = _uiGameObjectDict[UIPrefabType.UI_FocusContainer];
        goFocus.transform.SetSiblingIndex(goUI.transform.GetSiblingIndex() - 1);
    }

    private void moveFocusContainerToFirst()
    {
        GameObject goFocus = _uiGameObjectDict[UIPrefabType.UI_FocusContainer];
        goFocus.transform.SetAsFirstSibling();
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
        UI_GameOver,
        UI_Ingame,
        UI_BuildingSetting
    }

    //건물 업그레이드, 철거 버튼 기능 추가 및 활성화 비활성화
    [HideInInspector] public GameObject building;

    public void OnBuildingSetting(GameObject target)
    {
        //건물 설정할 오브젝트 설정
        this.building = target;
        
        //업그레이드, 취소, 철거 버튼 활성화
        Instance.AddUI(UIPrefabType.UI_BuildingSetting);

        RectTransform btnRect = _uiGameObjectDict[UIPrefabType.UI_BuildingSetting].GetComponent<RectTransform>();

        Vector2 screenPosition = Input.mousePosition;

        // UI 캔버스의 로컬 좌표 얻기
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Instance.GetComponent<RectTransform>(), screenPosition, null, out Vector2 localPoint);

        Debug.Log(btnRect);
        Debug.Log(localPoint); 
        // UI 요소 이동
        btnRect.anchoredPosition = localPoint;
    }

    public void OnUpgraded()
    {
        if (building == null)
        {
            Debug.Log("빌딩이 설정되어 있지 않음");
            return;
        }
            
        AbstractConstruct buildingscript = building.GetComponent<AbstractConstruct>();
        buildingscript.OnUpgrade();

        Instance.RemoveUI();
    }

    public void OnBuildingSettingCanceled()
    {
        building = null;
        Instance.RemoveUI();
    }

    public void OnDestroyed()
    {
        if (building == null)
        {
            Debug.Log("빌딩이 설정되어 있지 않음");
            return;
        }

        AbstractConstruct buildingscript = building.GetComponent<AbstractConstruct>();
        buildingscript.DestroyTower();

        building = null;
        Instance.RemoveUI();
    }
    [Header("Ingame")]
    [SerializeField] private GameObject tonedownimage;
    public void SetCanvasToneDown()
    {
        //톤 다운 이미지 활성화
        tonedownimage.SetActive(true);
    }

    public void SetCanvasToneUp()
    {
        //톤 업 이미지 비활성화
        tonedownimage.SetActive(false);
    }
}