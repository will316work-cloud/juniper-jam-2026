using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class SfxPooler
{
    [Header("Audio Pool")]
    int _basePoolSize;
    int _extendSize; 
    MonoBehaviour _monoBehaviour;
    Transform _parent;
    
    Dictionary<AudioType, AudioClip> _soundLookupDictionary = new();
    List<AudioSource> _pool = new(); 
    float _volume;
    SfxVolumeSettings[] _sfxVolumeSettings;

    public void Initialize(SfxPoolData data, MonoBehaviour monoBehaviour,SfxVolumeSettings[] sfxVolumeSettings)
    {
        _monoBehaviour = monoBehaviour;
        _basePoolSize = data.BasePoolSize;
        _parent = data.Parent;
        _sfxVolumeSettings = sfxVolumeSettings;

        CreateLookupDictionary_Sound(data.SfxEntryList);
        CreateBasePool();
    }

    public AudioSource CreateNewSource()
    {
        GameObject obj = new("Audio Source");
        AudioSource src = obj.AddComponent<AudioSource>();
        src.volume = 0.7f;
        src.playOnAwake = false;
        src.transform.SetParent(_parent);
        return src;
    } 

    void CreateBasePool()
    {
        for(int i = 0; i < _basePoolSize; i++) Enqueue(CreateNewSource());
        _extendSize = _basePoolSize;
    }

    void CreateLookupDictionary_Sound(SfxEntryList audioEntryList)
    {
        foreach(AudioEntry entry in audioEntryList.AudioList) _soundLookupDictionary.Add(entry.Type, entry.Clip);
    }

    void Enqueue(AudioSource src)
    {
        _pool.Add(src);
    }

    AudioSource Dequeue()
    {
        if(_pool.Count == 0) Extend();

        AudioSource src = _pool[0];
        _pool.RemoveAt(0);
        return src;
    }

    void Extend()
    {
        for(int i = 0; i < _extendSize; i++) Enqueue(CreateNewSource());
    }

    public void GetAudio(AudioType audioType, bool randomPitch = true)
    {
        AudioSource src = Dequeue();
        if(randomPitch) src.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        else src.pitch = 1f;
        src.clip = _soundLookupDictionary[audioType];
        src.volume = GetVolume(audioType);

        _monoBehaviour.StartCoroutine(WaitForAudioToFinishAndEnqueue(src));
    }

    IEnumerator WaitForAudioToFinishAndEnqueue(AudioSource src)
    {
        src.Play();
        yield return new WaitUntil(() => !src.isPlaying);
        Enqueue(src);
    }

    float GetVolume(AudioType type)
    {
        foreach(SfxVolumeSettings setting in _sfxVolumeSettings)
        {
            if(setting.Type == type)
                return setting.Volume * _volume; 
        }

        return 0.7f * _volume;
    }

    public void SetOverallVolume(float volume) => _volume = volume;
}

[Serializable]
public class SfxPoolData
{
    public SfxEntryList SfxEntryList;
    public int BasePoolSize;
    public Transform Parent;
}