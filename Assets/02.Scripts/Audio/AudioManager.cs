using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts;
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

    [SerializeField] private SettingsSO _setting;
    
    
    [Header("Audio control")]
    [SerializeField] private AudioMixer audioMixer = default;
    [Range(0f, 1f)]
    [SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;

    [SerializeField] private AudioVolumeChannelSO MainVolumeChannelSO;
    [SerializeField] private AudioVolumeChannelSO EffectVolumeChannelSO;
    [SerializeField] private AudioVolumeChannelSO MusicVolumeChannelSO;

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
        
    }

    private void OnEnable()
    {
        EffectChannel.OnAudioPlayRequested += PlayAudioQ;
        MusicChannel.OnAudioPlayRequested += PlayMusic;
        MainVolumeChannelSO.OnVolumeChanged += ChangeMasterVolume;
        EffectVolumeChannelSO.OnVolumeChanged += ChangeSfxVolume;
        MusicVolumeChannelSO.OnVolumeChanged += ChangeMusicVolume;
    }

    private void OnDisable()
    {
        EffectChannel.OnAudioPlayRequested -= PlayAudioQ;
        MusicChannel.OnAudioPlayRequested -= PlayMusic;
        MainVolumeChannelSO.OnVolumeChanged -= ChangeMasterVolume;
        EffectVolumeChannelSO.OnVolumeChanged -= ChangeSfxVolume;
        MusicVolumeChannelSO.OnVolumeChanged -= ChangeMusicVolume;
    }


    private void Start()
    {
#if UNITY_EDITOR
        if (_setting)
        {
            _masterVolume = _setting.MasterVolume;
            _musicVolume = _setting.MusicVolume;
            _sfxVolume = _setting.SfxVolume;
        }
#endif

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

    private void ChangeMasterVolume(float value)
    {
       _masterVolume = value;
        SetGroupVolume("MasterVolume",value);
    }
    private void ChangeMusicVolume(float value)
    {
        _musicVolume = value;
        SetGroupVolume("MusicVolume",value);
    }
    private void ChangeSfxVolume(float value)
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
