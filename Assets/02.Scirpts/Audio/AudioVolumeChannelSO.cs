using UnityEngine;

namespace _02.Scirpts.Audio
{
    /// <summary>
    /// 볼륨 변경 중개자
    /// </summary>
    [CreateAssetMenu(fileName = "NewAudioVolumeChannel", menuName = "Event/AudioVolumeChannel")]
    public class AudioVolumeChannelSO : ScriptableObject
    {
        public ChangeVolume OnVolumeChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">0.0~1.0</param>
        public void RaisePlayEvent(float volume)
        {
            OnVolumeChanged?.Invoke(volume);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">0.0~1.0</param>
        public delegate void ChangeVolume(float value);
    }
}