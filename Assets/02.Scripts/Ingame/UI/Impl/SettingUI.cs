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

        [SerializeField] private GameObject _gameQuitButton;

        [SerializeField] private Slider masterVolSlider;
        [SerializeField] private Slider musicVolSlider;
        [SerializeField] private Slider sfxVolSlider;

        private void OnEnable()
        {
            masterVolSlider.value = _setting.MasterVolume;
            musicVolSlider.value = _setting.MusicVolume;
            sfxVolSlider.value = _setting.SfxVolume;

            masterVolSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolSlider.onValueChanged.AddListener(SetSfxVolume);
            
        }

        private void SetMasterVolume(float n) => _setting.MasterVolume = n;
        private void SetMusicVolume(float n) => _setting.MusicVolume = n;
        private void SetSfxVolume(float n) => _setting.SfxVolume = n;
        
        private void OnDisable()
        {
            masterVolSlider.onValueChanged.RemoveListener(SetMasterVolume);
            musicVolSlider.onValueChanged.RemoveListener(SetMusicVolume);
            sfxVolSlider.onValueChanged.RemoveListener(SetSfxVolume);
            
            _setting.Save();
        }


        public void setGameQuitButtonVisibility(bool visible)
        {
            _gameQuitButton.SetActive(visible);
        }


    }
}