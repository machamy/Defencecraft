using System;
using _02.Scirpts.Audio;
using UnityEngine;

namespace _02.Scirpts
{
    
    /// <summary>
    /// 설정값 공유용 SO
    /// </summary>
    /// <remarks>
    /// 사운드 변경은 대문자로 시작하는 프라퍼티를 수정하면 자동으로 된다.
    /// </remarks>
    [CreateAssetMenu(menuName = "ScriptableObject/SettingSO", fileName = "SettingsSO")]
    public class SettingsSO : ScriptableObject
    {
        [SerializeField][Range(0f, 1f)] private float masterVolume = 0.7f;
        [SerializeField][Range(0f, 1f)] private float musicVolume = 0.7f;
        [SerializeField][Range(0f, 1f)] private float sfxVolume = 0.7f;

        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = value;
                masterVolumeChannel.RaiseVolumeEvent(value);
            }
        }

        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                musicVolume = value;
                musicVolumeChannel.RaiseVolumeEvent(value);
            }
        }

        public float SfxVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = value;
                sfxVolumeChannel.RaiseVolumeEvent(value);
            }
        }

        [SerializeField] private AudioVolumeChannelSO masterVolumeChannel;
        [SerializeField] private AudioVolumeChannelSO musicVolumeChannel;
        [SerializeField] private AudioVolumeChannelSO sfxVolumeChannel;


        public void Save()
        {
            PlayerPrefs.SetFloat("volume_master", masterVolume);
            PlayerPrefs.SetFloat("volume_music", musicVolume);
            PlayerPrefs.SetFloat("volume_sfx", sfxVolume);
            
            PlayerPrefs.Save();
        }

        public void Load()
        {
            Debug.Log("[SettingSO] Loading Data");
            masterVolume = PlayerPrefs.GetFloat("volume_master", masterVolume);
            musicVolume=PlayerPrefs.GetFloat("volume_music", musicVolume);
            sfxVolume=PlayerPrefs.GetFloat("volume_sfx", sfxVolume);
        }
    }
}