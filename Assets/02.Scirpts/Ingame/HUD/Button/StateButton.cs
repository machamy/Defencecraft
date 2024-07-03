﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _02.Scirpts.Ingame.HUD.Button
{
    public class StateButton : BaseButton
    {

        private int n;
        [SerializeField] private List<UnityEvent> events;
        [SerializeField] private List<Sprite> buttonSprites;
        [SerializeField] private List<Sprite> buttonPressedSprites;
        

        private void Awake()
        {
            n = 0;
            changeSprite();
            OnPointerDownEvent.AddListener((eventData) => OnPressDown());
            OnPointerUpEvent.AddListener((eventData) => OnPressUp());
            
            Debug.Log("등록한 곳 ID : " + );
        }

        private void OnPressDown()
        {
            GetComponent<Image>().sprite = buttonPressedSprites[n];
        }

        private void OnPressUp()
        {
            n++;
            if (n >= events.Count)
                n = 0;
            
            events[n].Invoke();
            GetComponent<Image>().sprite = buttonSprites[n];
        }


        private void changeSprite()
        {
            GetComponent<Image>().sprite = buttonSprites[n];
        }
    }
}