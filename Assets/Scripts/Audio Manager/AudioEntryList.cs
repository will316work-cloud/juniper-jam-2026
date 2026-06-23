using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Audio Entry List", menuName = "Data/Pooling/Audio Entry List")]
public class SfxEntryList : ScriptableObject
{
    public List<AudioEntry> AudioList;
}