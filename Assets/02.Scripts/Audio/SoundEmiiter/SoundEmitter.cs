using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _02.Scirpts.Audio
{
    /// <summary>
    /// 소리를 재생하는 주체
    /// </summary>
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        public event UnityAction<SoundEmitter> OnSoundFinished;

        private void Awake()
        {
            _audioSource = this.GetComponent<AudioSource>();
        }

        /// <summary>
        /// 오디오 클립을 재생시킨다. 3차원 공간.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="setting"></param>
        /// <param name="isLoop"></param>
        /// <param name="pos"></param>
        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO setting, bool isLoop, Vector3 pos = default)
        {
            _audioSource.clip = clip;

            _audioSource.transform.position = pos;
            _audioSource.loop = isLoop;
            setting.ApplyTo(_audioSource);
            _audioSource.Play();

            if (!isLoop)
            {
                StartCoroutine(SoundTimer(clip.length));
            }
        }

        public void Resume()
        {
            _audioSource.Play();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// 루프가 진행중일경우 루프를 멈춘다
        /// </summary>
        public void Finish()
        {
            if (_audioSource.loop)
            {
                _audioSource.loop = false;
            }
        }

        private IEnumerator SoundTimer(float time)
        {
            yield return new WaitForSeconds(time);
            
            NotifyFinished();
        }

        public bool IsPlaying => _audioSource.isPlaying;
        public bool IsLooping => _audioSource.loop;

        public AudioClip CurrentlyPlayingAudio => _audioSource.clip;

        private void NotifyFinished()
        {
            OnSoundFinished.Invoke(this);
        }
    }




}