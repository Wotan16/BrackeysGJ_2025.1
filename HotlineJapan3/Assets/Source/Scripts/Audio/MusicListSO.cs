using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicList", menuName = "Scriptable Objects/MusicList")]
public class MusicListSO : ScriptableObject
{
    [Serializable]
    public struct TypeMusicPair
    {
        public MusicType type;
        public AudioClip clip;
    }

    public List<TypeMusicPair> list;

    public AudioClip GetMusicClip(MusicType type)
    {
        foreach (var pair in list)
        {
            if (pair.type == type)
                return pair.clip;
        }

        Debug.LogError("No matching audio clip found for MusicType." + type.ToString());
        return null;
    }
}
