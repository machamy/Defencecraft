using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

/// <summary>
/// 오디오 관련 전부 관리하는 매니저
/// TODO : 싱글턴처럼 쓸 수 있게 할 예정
/// </summary>
public class AudioManager : MonoBehaviour
{
    
    [SerializeField] private SoundEmitterPoolSO emitterPoolSO;
    [SerializeField] private int _initSize = 10;

    [SerializeField] private AudioChannelSO EffectChannel = default;
    [SerializeField] private AudioChannelSO MusicChannel = default;
    
    
    [Header("Audio control")]
    [SerializeField] private AudioMixer audioMixer = default;
    [Range(0f, 1f)]
    [SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;


    private SoundEmitter musicEmitter = null;


    [FormerlySerializedAs("TestQueue")] public AudioQueueSO testQueueSo;
    public AudioConfigurationSO TestConfig;

    public void TestPlay()
    {
        PlayAudioQ(testQueueSo,TestConfig);
    }
    
    private void Awake()
    {
        emitterPoolSO.init(_initSize);
        emitterPoolSO.SetParent(this.transform);
        
        EffectChannel.OnAudioPlayRequested += PlayAudioQ;
        MusicChannel.OnAudioPlayRequested += PlayMusic;
    }

    private void Start()
    {
        ChangeMasterVolume(_masterVolume);
        ChangeMusicVolume(_musicVolume);
        ChangeSfxVolume(_sfxVolume);
    }

    /// <summary>
    /// 에디터상에서 변경해도 적용되게
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            ChangeMasterVolume(_masterVolume);
            ChangeMusicVolume(_musicVolume);
            ChangeSfxVolume(_sfxVolume);
        }
    }

    void ChangeMasterVolume(float value)
    {
        _masterVolume = value;
        SetGroupVolume("MasterVolume",value);
    }
    void ChangeMusicVolume(float value)
    {
        _musicVolume = value;
        SetGroupVolume("MusicVolume",value);
    }
    void ChangeSfxVolume(float value)
    {
        _sfxVolume = value;
        SetGroupVolume("SfxVolume",value);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueSo"></param>
    /// <param name="config"></param>
    /// <param name="pos"> 무시되는 값</param>
    private void PlayMusic(AudioQueueSO queueSo, AudioConfigurationSO config, Vector3 pos = default)
    {
        AudioClip audio = queueSo.GetClip();
        if (musicEmitter != null && musicEmitter.IsPlaying)
        {
            if (musicEmitter.CurrentlyPlayingAudio == audio) // 이미 재생중이면 무시
                return;
            musicEmitter.Stop();
            emitterPoolSO.Return(musicEmitter);
        }

        musicEmitter = emitterPoolSO.Get();
        musicEmitter.PlayAudioClip(audio,config,true);
    }


    public void PlayAudioQ(AudioQueueSO q, AudioConfigurationSO config, Vector3 pos = default)
    {
        AudioClip song = q.GetClip();
        SoundEmitter emitter = emitterPoolSO.Get();
        emitter.PlayAudioClip(song,config,q.looping,pos);
        if (!q.looping)
        {
            emitter.OnSoundFinished += StopCleanEmitter;
        }
    }
    
    
    
    private void SetGroupVolume(string name, float val)
    {
        if (!audioMixer.SetFloat(name, NormalizedToMixerVal(val)))
        {
            Debug.LogError($"{name} Group을 찾을 수 없음");
        };
    }

    /// <summary>
    /// 0~1 값을 -80~0 값으로 변경
    /// </summary>
    /// <param name="normal"></param>
    /// <returns></returns>
    private float NormalizedToMixerVal(float normal)
    {
        // TODO : log10 이용해서 하는 방법도 있음
        return (normal - 1f) * 80f;
    }

    private void StopCleanEmitter(SoundEmitter emitter)
    {
        if (!emitter.IsLooping) // 반복이 아닌경우
        {
            emitter.OnSoundFinished -= StopCleanEmitter;
        }
        emitter.Stop();
        emitterPoolSO.Return(emitter);
    }
    
}
