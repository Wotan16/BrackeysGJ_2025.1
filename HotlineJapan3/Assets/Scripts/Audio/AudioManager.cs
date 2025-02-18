using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MUSIC_PARAM_NAME = "MusicVolume";
    private const string SOUND_PARAM_NAME = "SoundVolume";

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    private enum MixerGroupType
    {
        Master,
        Music,
        Sound
    }

    [Serializable]
    private struct GroupTypePair
    {
        public MixerGroupType type;
        public AudioMixerGroup group;
    }

    [SerializeField] private List<GroupTypePair> mixerGroups;
    [SerializeField] private SoundSOList soundSOList;
    [SerializeField] private MusicListSO musicList;
    private AudioSource musicSource;
    private AudioSource nextMusicSource;
    [SerializeField] private float timeToBlendMusic;
    private float timeToBlendDelta;

    private void Awake()
    {
        InitializeSingleton();
        DontDestroyOnLoad(gameObject);
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one " + GetType().Name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetMixerFloat(MUSIC_PARAM_NAME, AudioSettings.musicVolume.Value);
        SetMixerFloat(SOUND_PARAM_NAME, AudioSettings.soundVolume.Value);

        AudioSettings.soundVolume.OnValueChanged += SoundVolume_OnValueChanged;
        AudioSettings.musicVolume.OnValueChanged += MusicVolume_OnValueChanged;
    }


    private void MusicVolume_OnValueChanged(float value)
    {
        SetMixerFloat(MUSIC_PARAM_NAME, value);
    }

    private void SoundVolume_OnValueChanged(float value)
    {
        SetMixerFloat(SOUND_PARAM_NAME, value);
    }

    private void SetMixerFloat(string name, float value)
    {
        float newValue = Mathf.Lerp(-80f, 20f, value);
        audioMixer.SetFloat(name, newValue);
    }

    private void Update()
    {
        if (nextMusicSource == null)
            return;

        timeToBlendDelta -= Time.deltaTime;
        float oldVolume = timeToBlendDelta / timeToBlendMusic;
        oldVolume = Mathf.Clamp01(oldVolume);
        float nextVolume = 1 - oldVolume;
        musicSource.volume = oldVolume;
        nextMusicSource.volume = nextVolume;

        if (timeToBlendDelta <= 0)
        {
            Destroy(musicSource.gameObject);
            musicSource = nextMusicSource;
            nextMusicSource = null;
        }
    }

    public static void PlayMusic(MusicType type)
    {
        GameObject sourceGO = new GameObject("Music Source");
        AudioSource musicSource = sourceGO.AddComponent<AudioSource>();
        musicSource.resource = Instance.musicList.GetMusicClip(type);
        musicSource.outputAudioMixerGroup = GetMixerGroupByType(MixerGroupType.Music);
        musicSource.loop = true;
        musicSource.Play();

        if (Instance.musicSource == null)
        {
            Instance.musicSource = musicSource;
            return;
        }

        if (Instance.nextMusicSource != null)
        {
            Destroy(Instance.musicSource.gameObject);
            Instance.musicSource = Instance.nextMusicSource;
            Instance.nextMusicSource = musicSource;
            Instance.timeToBlendDelta = Instance.timeToBlendMusic;
            return;
        }

        Instance.timeToBlendDelta = Instance.timeToBlendMusic;
        Instance.nextMusicSource = musicSource;
        Instance.timeToBlendDelta = Instance.timeToBlendMusic;
    }

    private static AudioMixerGroup GetMixerGroupByType(MixerGroupType type)
    {
        foreach(GroupTypePair pair in Instance.mixerGroups)
        {
            if(pair.type == type)
                return pair.group;
        }

        Debug.LogError("No mixer group found. Type: " + type.ToString());
        return null;
    }

    private static AudioClip GetAudioClipByType(SoundType type)
    {
        SoundSO soundSO = GetSoundSOByType(type);

        if(soundSO == null)
            return null;

        if (soundSO.singleSound)
            return soundSO.defaultSound;
        else
            return GetRandomAudioClip(soundSO);
    }

    private static AudioClip GetRandomAudioClip(SoundSO soundSO)
    {
        int randomIndex = UnityEngine.Random.Range(0, soundSO.sounds.Count);
        return soundSO.sounds[randomIndex];
    }

    public static SoundSO GetSoundSOByType(SoundType type)
    {
        foreach (SoundSO s in Instance.soundSOList.soundList)
        {
            if (s.soundType == type)
            {
                return s;
            }
        }

        Debug.LogError("No matching SoundSO found. Type: " + type.ToString());
        return null;
    }

    public static void PlaySound(SoundType type)
    {
        SoundSO soundSO = GetSoundSOByType(type);
        GameObject sourceGO = new GameObject(type.ToString() + " Source");
        AudioSource source = sourceGO.AddComponent<AudioSource>();

        source.outputAudioMixerGroup = GetMixerGroupByType(MixerGroupType.Sound);
        source.volume = soundSO.volume;
        if (soundSO.randomPitch)
        {
            float pitchValue = soundSO.pitch + UnityEngine.Random.Range(-soundSO.maxPitchDistortion, soundSO.maxPitchDistortion);
            source.pitch = pitchValue;
        }
        else
        {
            source.pitch = soundSO.pitch;
        }

        AudioClip clip = GetAudioClipByType(type);
        source.PlayOneShot(clip);
        Destroy(sourceGO, clip.length);
        
    }

    

    private void OnDestroy()
    {
        AudioSettings.soundVolume.OnValueChanged -= SoundVolume_OnValueChanged;
        AudioSettings.musicVolume.OnValueChanged -= MusicVolume_OnValueChanged;
    }
}
