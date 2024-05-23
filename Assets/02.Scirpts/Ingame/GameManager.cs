using _02.Scirpts.Ingame;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{

    public World world;
    
    public bool isPaused;
    public float prevTimeScale;

    public enum GameState{ Start, Pause }
    
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;
    
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
        }
        else
        {
            isPaused = false;
            Time.timeScale = prevTimeScale;
            OnGameResumed.Invoke();
        }
        
        
    }
}