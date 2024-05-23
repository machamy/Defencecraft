using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _02.Scirpts.Ingame.HUD.Button
{
    public class StateButton : BaseButton
    {

        private int n;
        [SerializeField] private List<UnityEvent> events;
        [SerializeField] private List<Color> buttonSprites;
        

        private void Awake()
        {
            n = 0;
            changeSprite();
            OnClickEvent.AddListener(() => OnPress());
        }

        private void OnPress()
        {
            n++;
            if (n >= events.Count)
                n = 0;
            events[n].Invoke();
            Debug.Log(n);
            changeSprite();
        }


        private void changeSprite()
        {
            GetComponent<Image>().color = buttonSprites[n];
        }
    }
}