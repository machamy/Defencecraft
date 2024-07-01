using System;
using UnityEngine;

namespace _02.Scirpts
{
    
    /// <summary>
    /// 설정값 공유용 SO
    /// </summary>
    /// <remarks>
    /// 여기 있는 값은 실시간으로 변경되더라도 반영되지 않음
    /// </remarks>
    [CreateAssetMenu(menuName = "ScriptableObject/SettingSO", fileName = "SettingsSO")]
    public class SettingsSO : ScriptableObject
    {
        [Range(0f, 1f)] public float masterVolume = 0.7f;
        [Range(0f, 1f)] public float musicVolume = 0.7f;
        [Range(0f, 1f)] public float sfxVolume = 0.7f;

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