using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class AudioPool
{
    [Header("Audio Pool")]
    int _basePoolSize;
    int _extendSize; 
    MonoBehaviour _monoBehaviour;
    Transform _parent;
    
    Dictionary<AudioType, AudioClip> _soundLookupDictionary = new();
    List<AudioSource> _pool = new(); 
    AudioSource _songSource;

    public void StartMainMenuMusic()
    {
        _songSource.volume = 0.7f;
        _songSource.clip = _soundLookupDictionary[AudioType.Song_01];
        _songSource.Play();
    }
    public void StopSong() => _songSource.DOFade(0f, 2f).OnComplete(() => _songSource.Stop());

    public void Initialize(AudioPoolData data, MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        _basePoolSize = data.BasePoolSize;
        _parent = data.Parent;

        CreateLookupDictionary_Sound(data.AudioEntryList);
        CreateBasePool();
        _songSource = Dequeue();

        StartMainMenuMusic();
    }

    AudioSource CreateNewSource()
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

    void CreateLookupDictionary_Sound(AudioEntryList audioEntryList)
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
        
        _monoBehaviour.StartCoroutine(WaitForAudioToFinishAndEnqueue(src));
    }

    // public AudioSource PlaySong(LoopedAudioType audioType, bool fadeIn = false)
    // {
    //     AudioSource src = Dequeue();
    //     src.clip = _songLookupDictionary[audioType];
    //     src.loop = true;

    //     if(fadeIn)
    //     {
    //         src.volume = 0;
    //         src.Play();
    //         src.DOFade(0.7f, 1f);
    //     }

    //     _monoBehaviour.StartCoroutine(WaitForAudioToFinishAndEnqueue(src));
    //     return src;
    // }

    IEnumerator WaitForAudioToFinishAndEnqueue(AudioSource src)
    {
        src.Play();
        yield return new WaitUntil(() => !src.isPlaying);
        Enqueue(src);
    }
}

[Serializable]
public class AudioPoolData
{
    public AudioEntryList AudioEntryList;
    public int BasePoolSize;
    public Transform Parent;
}