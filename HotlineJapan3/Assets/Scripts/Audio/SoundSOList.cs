using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSOList", menuName = "Scriptable Objects/SoundSOList")]
public class SoundSOList : ScriptableObject
{
    public List<SoundSO> soundList;
}
