using UnityEngine;

namespace _02.Scirpts.Audio
{
    /// <summary>
    /// 음악을 재생하는 중개자
    /// </summary>
    [CreateAssetMenu(fileName = "NewAudioChannel", menuName = "Event/AudioChannel")]
    public class AudioChannelSO : ScriptableObject
    {
        public AudioPlayAction OnAudioPlayRequested;

        public void RaisePlayEvent(AudioQueueSO audioQueue, AudioConfigurationSO config, Vector3 position = default)
        {
            OnAudioPlayRequested?.Invoke(audioQueue,config,position);
        }


        public delegate void AudioPlayAction(AudioQueueSO audioQueue, AudioConfigurationSO config, Vector3 position);
    }
}