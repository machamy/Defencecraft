using _02.Scirpts.Ingame;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public World world;
    
    public bool isPaused;
    public float prevTimeScale;

    
    public void SetGameSpeed(float gameSpeed)
    {
        Time.timeScale = gameSpeed;
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            isPaused = true;
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            isPaused = false;
            Time.timeScale = prevTimeScale;
        }
    }
}