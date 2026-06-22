using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SongPooler
{
    private Transform _sourceParent;
    private AudioSource _gameplaySource_01;
    private AudioSource _gameplaySource_02;
    private AudioSource _currentSource;
    private AudioSource _previousSource;
    private AudioSource _mainMenuSource;
    private List<AudioClip> _availableGameplaySongs = new(); 
    private List<AudioClip> _usedGamePlaySongs = new(); 
    private AudioClip _mainMenuSong;
    private float _crossfadeLength;
    private MonoBehaviour _monoBehaviour;
    private float _volume = 0.7f;
    private Coroutine _gameplayCoroutine;
    private AudioClip _previousClip;

    public void Initialize(SongPoolerData data, MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        _sourceParent = data.SourceParent;
        _mainMenuSong = data.MainMenuSong;
        _crossfadeLength = data.CrossfadeLength;
        _availableGameplaySongs.AddRange(data.GameplaySongs);

        CreateAudioSources();
    }

    void CreateAudioSources()
    {
        _gameplaySource_01 = CreateAudioSource();
        _gameplaySource_02 = CreateAudioSource();

        _mainMenuSource = CreateAudioSource();
        _mainMenuSource.loop = true;
        _mainMenuSource.clip = _mainMenuSong;
    }

    AudioSource CreateAudioSource()
    {
        AudioSource src = new GameObject("Audio Source").AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.transform.SetParent(_sourceParent);
        return src;
    }

    public void StartGameplayMusic()
    {
        _gameplayCoroutine = _monoBehaviour.StartCoroutine(GameplayMusicRoutine());
    }

    IEnumerator GameplayMusicRoutine()
    {
        _currentSource = _gameplaySource_01;
        _currentSource.clip = GetRandomGameplaySong();
        _currentSource.volume = 0;
        _currentSource.Play();
        _currentSource.DOFade(_volume, _crossfadeLength);

        while (true)
        {
            // Wait until it's time to start the crossfade
            yield return new WaitForSeconds(_currentSource.clip.length - _crossfadeLength);

            // Swap sources
            _previousSource = _currentSource;
            _currentSource = (_currentSource == _gameplaySource_01) ? _gameplaySource_02 : _gameplaySource_01;

            // Fade out old, fade in new
            _previousSource.DOFade(0, _crossfadeLength).OnComplete(() => _previousSource.Stop());
            
            _currentSource.clip = GetRandomGameplaySong();
            _currentSource.volume = 0;
            _currentSource.Play();
            _currentSource.DOFade(_volume, _crossfadeLength);

            // Wait for the crossfade window to pass before checking again
            yield return new WaitForSeconds(_crossfadeLength);
        }
    }

    float SongLength(AudioClip clip) => clip.length;

    void ResetGameplaySongLists()
    {
        _availableGameplaySongs.AddRange(_usedGamePlaySongs);
        _usedGamePlaySongs.Clear();
    }

    public void FadeToNextSong()
    {
        if (_gameplayCoroutine != null)
            _monoBehaviour.StopCoroutine(_gameplayCoroutine);
        _gameplayCoroutine = _monoBehaviour.StartCoroutine(FadeToNextSongRoutine());
    }

    IEnumerator FadeToNextSongRoutine()
    {
        _previousSource = _currentSource;
        _currentSource = (_currentSource == _gameplaySource_01) ? _gameplaySource_02 : _gameplaySource_01;

        if (_previousSource != null && _previousSource.isPlaying)
            _previousSource.DOFade(0, _crossfadeLength).OnComplete(() => _previousSource.Stop());

        _currentSource.clip = GetRandomGameplaySong();
        _currentSource.volume = 0;
        _currentSource.Play();
        _currentSource.DOFade(_volume, _crossfadeLength);

        while (true)
        {
            yield return new WaitForSeconds(_currentSource.clip.length - _crossfadeLength);

            _previousSource = _currentSource;
            _currentSource = (_currentSource == _gameplaySource_01) ? _gameplaySource_02 : _gameplaySource_01;
            _previousSource.DOFade(0, _crossfadeLength).OnComplete(() => _previousSource.Stop());

            _currentSource.clip = GetRandomGameplaySong();
            _currentSource.volume = 0;
            _currentSource.Play();
            _currentSource.DOFade(_volume, _crossfadeLength);

            yield return new WaitForSeconds(_crossfadeLength);
        }
    }

    public void StartMenuMusic()
    {
        _mainMenuSource.volume = 0;
        _mainMenuSource.Play();
        _mainMenuSource.DOFade(_volume, _crossfadeLength);

        if (_currentSource != null && _currentSource.isPlaying)
        {
            _currentSource.DOFade(0, _crossfadeLength).OnComplete(() =>
            {
                _currentSource.Stop();
                _monoBehaviour.StopCoroutine(_gameplayCoroutine);
            });
        }
    }
    AudioClip GetRandomGameplaySong()
    {
        if (_availableGameplaySongs.Count == 0)
            ResetGameplaySongLists();

        int index = UnityEngine.Random.Range(0, _availableGameplaySongs.Count);
        AudioClip song = _availableGameplaySongs[index];
        if (_previousClip == song && _availableGameplaySongs.Count > 1) return GetRandomGameplaySong();
        _availableGameplaySongs.RemoveAt(index);
        _usedGamePlaySongs.Add(song);
        _previousClip = song;
        return song;
    }
}

[Serializable]
public class SongPoolerData
{
    public Transform SourceParent;
    public float CrossfadeLength;
    public AudioClip MainMenuSong;
    public List<AudioClip> GameplaySongs = new();
}

