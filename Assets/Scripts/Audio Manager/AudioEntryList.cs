using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Audio Entry List", menuName = "Data/Pooling/Audio Entry List")]
public class AudioEntryList : ScriptableObject
{
    public List<AudioEntry> AudioList;
}