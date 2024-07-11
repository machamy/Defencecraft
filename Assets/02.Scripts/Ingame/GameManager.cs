using System;
using _02.Scirpts;
using _02.Scirpts.Ingame;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// 게임의 전체적인 것을 총괄하는 클래스
/// </summary>
public class GameManager : Singleton<GameManager>
{

    [Header("게임관련")]
    public World world;
    
    public bool isPaused;
    public float prevTimeScale;

    public enum GameState{ Start, Pause }
    
    
    [Header("게임 이벤트")]
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;

    [Header("설정")] 
    [SerializeField] private SettingsSO _settingsSo;

    public void SetSettingSO(SettingsSO so) => _settingsSo = so;
    
    
    
    public void Start()
    {
        EditorInitializer.activated = true;
    }

    /// <summary>
    /// 게임 속도를 조정합니다.
    /// </summary>
    /// <param name="gameSpeed">조정할 배속</param>
    public void SetGameSpeed(float gameSpeed)
    {
        Time.timeScale = gameSpeed;
    }

    /// <summary>
    /// 게임을 정지합니다.
    /// </summary>
    /// <param name="pause">true 시 정지</param>
    public void Pause(bool pause)
    {
        if (pause)
        {
            isPaused = true; 
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            OnGamePaused.Invoke();
            // TODO : 항상 이렇게 해야하나?
        }
        else
        {
            isPaused = false;
            Time.timeScale = prevTimeScale;
            OnGameResumed.Invoke();
        }
    }

    /// <summary>
    /// 볼륨 초기화
    /// </summary>
    private void InitializeVolumes()
    {
        _settingsSo.MasterVolume = 0.7f;
        _settingsSo.MusicVolume = 0.7f;
        _settingsSo.SfxVolume = 0.7f;
    }

    /// <summary>
    /// 게임 초기화
    /// </summary>
    public void InitializeGame()
    {
        InitializeVolumes();
            
        PlayerPrefs.DeleteAll();
        
        
        /*
         * 월드 클리어 데이터 지우기
         * 기타 게임 데이터 지우기
         */
        
        _settingsSo.Load(); // 초기 값 불러오기.
        SceneManager.LoadScene(0);
    }


    public enum Difficulty
    {
        Easy=0,
        Normal=1,
        Hard=2
    }
}