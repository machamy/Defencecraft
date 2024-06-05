using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scirpts.Audio
{
    /// <summary>
    /// 음악 데이터가 저장되는 SO
    /// </summary>
    [CreateAssetMenu(fileName = "NewAudioQueue", menuName = "Audio/AudioQueue")]
    public class AudioQueueSO : ScriptableObject
    {
        public bool looping = false;
        
        [SerializeField] private AudioClipGroup ClipGroup; // 배열은 필요없을듯함

        /// <summary>
        /// 오디오 그룹의 규칙대로 다음 클립을 가져온다
        /// </summary>
        /// <returns></returns>
        public AudioClip GetClip()
        {
            return ClipGroup.GetNext();
        }
    }

    [Serializable]
    public class AudioClipGroup
    {
        public QueueType queueType = QueueType.RandomIgnoreSelf;
        
        [SerializeField] private AudioClip[] _clips;


        private int nextPlayIdx = -1;
        private int prevPlayedIdx = -1;
        
        public enum QueueType
        {
            Sequence = 0,
            Random = 1,
            RandomIgnoreSelf = 2 // 자기 자신 재생 X
        }

        public AudioClip GetNext()
        {
            if (_clips.Length == 1)
                return _clips[0];
            if (nextPlayIdx == -1) // 최초 재생
            {
                nextPlayIdx = (queueType == QueueType.Sequence) ? 0 : Random.Range(0, _clips.Length);
            }
            else
            {
                switch (queueType)
                {
                    case QueueType.Sequence:
                        nextPlayIdx = (int)Mathf.Repeat(++nextPlayIdx, _clips.Length);
                        break;
                    case QueueType.Random:
                        Random.Range(0, _clips.Length);
                        break;
                    case QueueType.RandomIgnoreSelf:
                        do
                        {
                            nextPlayIdx = Random.Range(0, _clips.Length);
                        } while (nextPlayIdx == prevPlayedIdx);
                        break;
                }
            }
            prevPlayedIdx = nextPlayIdx;
            return _clips[nextPlayIdx];
        }
    }
}