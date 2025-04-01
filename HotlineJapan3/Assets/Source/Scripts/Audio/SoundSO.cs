using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
public class SoundSO : ScriptableObject
{
    public SoundType soundType;
    public List<AudioClip> sounds;
    public AudioClip defaultSound
    {
        get
        {
            if (sounds.Count == 0)
            {
                Debug.LogError("No sounds found in " + name);
                return null;
            }
            
                return sounds[0];
        }
    }

    public bool singleSound => sounds.Count == 1;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    public bool randomPitch = false;
    [Range(0f, 1f)] public float maxPitchDistortion = 0.1f;
}
