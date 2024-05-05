using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public InfoType infoType;
    public ButtonType buttonType;

    public enum InfoType
    {
        Health,
        Gold,
        RemainEnemy,
        RemainWave,
    }

    public enum ButtonType
    {
        Speed,
        Pause,
    }

    void Awake()
    {
        
    }

    void LateUpdate()
    {
        switch (infoType)
        {
            
        }
    }
}
