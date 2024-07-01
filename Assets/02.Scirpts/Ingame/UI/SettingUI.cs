using System;
using _02.Scirpts.Audio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _02.Scirpts.Ingame.UI
{
    public class SettingUI : BaseUI
    {
        /// <summary>
        /// 설정 SO
        /// </summary>
        [SerializeField] private SettingsSO _setting;
        
        [SerializeField] private AudioVolumeChannelSO masterVolumeChannel;
        [SerializeField] private AudioVolumeChannelSO musicVolumeChannel;
        [SerializeField] private AudioVolumeChannelSO sfxVolumeChannel;

        [SerializeField] private Slider masterVolSlider;
        [SerializeField] private Slider musicVolSlider;
        [SerializeField] private Slider sfxVolSlider;

        private void OnEnable()
        {
            masterVolSlider.value = _setting.masterVolume;
            musicVolSlider.value = _setting.musicVolume;
            sfxVolSlider.value = _setting.sfxVolume;
        }

        private void OnDisable()
        {
            _setting.Save();
            
            masterVolumeChannel.RaiseVolumeEvent(masterVolSlider.value);
            musicVolumeChannel.RaiseVolumeEvent(musicVolSlider.value);
            sfxVolumeChannel.RaiseVolumeEvent(sfxVolSlider.value);
        }

        private void InitializeVolumes()
        {
            masterVolumeChannel.RaiseVolumeEvent(0.7f);
            musicVolumeChannel.RaiseVolumeEvent(0.7f);
            sfxVolumeChannel.RaiseVolumeEvent(0.7f);
        }

        public void InitializeGame()
        {
            InitializeVolumes();
            
            PlayerPrefs.DeleteAll();
            _setting.Load(); // 초기 값 불러오기.
        
            /*
             * 월드 클리어 데이터 지우기
             * 기타 게임 데이터 지우기
             */
        }
    }
}