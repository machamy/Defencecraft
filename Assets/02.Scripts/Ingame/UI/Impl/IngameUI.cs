﻿using System;
using System.Collections;
using _02.Scirpts.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _02.Scirpts.Ingame.UI
{
    public class IngameUI : BaseUI
    {
        
        [SerializeField] private GameObject go_ingameParent;
        [SerializeField] private GameObject go_pauseParent;

        private EntityDamageSO EntityDamageChannel;
        
        [SerializeField] private TextMeshProUGUI text_Health;
        [SerializeField] private TextMeshProUGUI text_Gold;
        [SerializeField] private TextMeshProUGUI text_RemainEnemy;
        [SerializeField] private TextMeshProUGUI text_RemainWave;


        public Canvas Canvas;
        public DialogueController DialogueController;

        private GameManager _gameManager;
        
        public void Awake()
        {
            RegisterUIUpdate();
        }

        public void Start()
        {
            Canvas = FindObjectOfType<Canvas>();
            _gameManager = GameManager.Instance;
            _gameManager.OnGamePaused.AddListener(OnPaused);
            _gameManager.OnGameResumed.AddListener(OnResumed);
        }
        

        /// <summary>
        ///  OnGamePaused Subscribe - UI 숨길 거 숨기기
        /// </summary>
        /// <param name="paused"></param>
        public void OnPaused()
        {
            go_ingameParent.SetActive(false);
            go_pauseParent.SetActive(true);
        }
        
        
        /// <summary>
        /// OnGameResumed Subscribe - UI 보일 거 보이게
        /// </summary>
        public void OnResumed()
        {
            go_ingameParent.SetActive(true);
            go_pauseParent.SetActive(false);
        }


        public void RegisterUIUpdate()
        {
            if(EntityDamageChannel is not null)
                // 넥서스 데미지 이벤트 감지
                EntityDamageChannel.OnDamageEvent += (attacker, damaged, damage) =>
                {
                    if (damaged is Nexus nexus)
                    {
                        StartCoroutine(LateUpdateCoroutine(() => HealthTextUpdate(nexus)));
                    }
                        
                };
            
            // TODO : 골드 변화 이벤트 감지 후 GoldTextUpdate() 구독
            // TODO : 적 사망 감지 후 RemainEntityTextUpdate() 구독
            // TODO : 웨이브 구현 시 RemainWaveTextUpdate() 구독
            
        }
        
        public void HealthTextUpdate(Nexus attacker)
        {
            text_Health.text = "" + attacker.hp;
        }

        public void GoldTextUpdate()
        {
            // TODO : 골드 미구현
        }

        public void RemainEntityTextUpdate()
        {
            // TODO : 남은 적의 수 미구현
        }

        public void RemainWaveTextUpdate()
        {
            // TODO : 남은 웨이브 미구현
        }

        IEnumerator LateUpdateCoroutine(Action callback)
        {
            callback.Invoke();
            yield return null;
        }
        

    }
}