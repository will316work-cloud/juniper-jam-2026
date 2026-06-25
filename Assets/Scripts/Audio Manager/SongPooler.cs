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
    private AudioSource _gameOverSource;
    private AudioSource _dayChangeSource;
    
    private List<AudioClip> _availableGameplaySongs = new();
    private List<AudioClip> _usedGamePlaySongs = new();
    private float _crossfadeLength;
    private MonoBehaviour _monoBehaviour;
    private float _volume = 0.7f;
    private Coroutine _gameplayCoroutine;
    private AudioClip _previousClip;

    private AudioClip _mainMenuSong;
    private AudioClip _gameOverSong;
    private AudioClip _dayChangeSong;

    float _overallVolume;

    float TargetVolume => _volume * _overallVolume;

    public void SetOverallVolume(float volume)
    {
        _overallVolume = volume;
        _mainMenuSource.volume = TargetVolume;
        _gameplaySource_01.volume = TargetVolume;
        _gameplaySource_02.volume = TargetVolume;
    }

    public void Initialize(SongPoolerData data, MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        _sourceParent = data.SourceParent;
        _mainMenuSong = data.MainMenuSong;
        _gameOverSong = data.GameOverSong;
        _dayChangeSong = data.DayChangeSong;
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

        _gameOverSource = CreateAudioSource();
        _gameOverSource.loop = true;
        _gameOverSource.clip = _gameOverSong;

        _dayChangeSource = CreateAudioSource();
        _dayChangeSource.clip = _dayChangeSong;
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
        _currentSource.DOFade(TargetVolume, _crossfadeLength);

        while (true)
        {
            yield return new WaitForSeconds(_currentSource.clip.length - _crossfadeLength);

            _previousSource = _currentSource;
            _currentSource = (_currentSource == _gameplaySource_01) ? _gameplaySource_02 : _gameplaySource_01;

            var prevSrc = _previousSource;
            DOTween.Kill(prevSrc);
            prevSrc.DOFade(0, _crossfadeLength).OnComplete(() => prevSrc.Stop());

            DOTween.Kill(_currentSource);
            _currentSource.clip = GetRandomGameplaySong();
            _currentSource.volume = 0;
            _currentSource.Play();
            _currentSource.DOFade(TargetVolume, _crossfadeLength);

            yield return new WaitForSeconds(_crossfadeLength);
        }
    }

    void ResetGameplaySongLists()
    {
        _availableGameplaySongs.AddRange(_usedGamePlaySongs);
        _usedGamePlaySongs.Clear();
    }
    void FadeOutAllSources(AudioSource except)
    {
        if (_gameplayCoroutine != null)
        {
            _monoBehaviour.StopCoroutine(_gameplayCoroutine);
            _gameplayCoroutine = null;
        }

        TryFadeOut(_gameplaySource_01);
        TryFadeOut(_gameplaySource_02);
        if (_mainMenuSource != except) TryFadeOut(_mainMenuSource);
        if (_gameOverSource != except) TryFadeOut(_gameOverSource);
        if (_dayChangeSource != except) TryFadeOut(_dayChangeSource);
    }

    void TryFadeOut(AudioSource source)
    {
        DOTween.Kill(source);
        if (source.isPlaying)
            source.DOFade(0, _crossfadeLength).OnComplete(() => source.Stop());
    }

    public void FadeInMenuMusic()
    {
        FadeOutAllSources(_mainMenuSource);
        _mainMenuSource.volume = 0;
        _mainMenuSource.Play();
        _mainMenuSource.DOFade(TargetVolume, _crossfadeLength);
    }

    public void FadeOutMenuMusic()
    {
        TryFadeOut(_mainMenuSource);
    }

    public void FadeInGameOverMusic()
    {
        FadeOutAllSources(_gameOverSource);
        _gameOverSource.volume = 0;
        _gameOverSource.Play();
        _gameOverSource.DOFade(TargetVolume, _crossfadeLength);
    }

    public void FadeOutGameOverMusic()
    {
        TryFadeOut(_gameOverSource);
    }

    public void FadeInDayChangeMusic()
    {
        FadeOutAllSources(_dayChangeSource);
        _dayChangeSource.volume = 0;
        _dayChangeSource.Play();
        _dayChangeSource.DOFade(TargetVolume, _crossfadeLength / 2);
    }

    public void FadeOutDayChangeMusic()
    {
        TryFadeOut(_dayChangeSource);
    }

    public void FadeInGameplayMusic()
    {
        TryFadeOut(_mainMenuSource);
        TryFadeOut(_gameOverSource);
        TryFadeOut(_dayChangeSource);

        if (_gameplayCoroutine != null)
            _monoBehaviour.StopCoroutine(_gameplayCoroutine);

        _gameplayCoroutine = _monoBehaviour.StartCoroutine(FadeToNextSongRoutine());
    }

    IEnumerator FadeToNextSongRoutine()
    {
        _previousSource = _currentSource;
        _currentSource = (_currentSource == _gameplaySource_01) ? _gameplaySource_02 : _gameplaySource_01;

        if (_previousSource != null && _previousSource.isPlaying)
        {
            var prevSrc = _previousSource;
            DOTween.Kill(prevSrc);
            prevSrc.DOFade(0, _crossfadeLength).OnComplete(() => prevSrc.Stop());
        }

        DOTween.Kill(_currentSource);
        _currentSource.clip = GetRandomGameplaySong();
        _currentSource.volume = 0;
        _currentSource.Play();
        _currentSource.DOFade(TargetVolume, _crossfadeLength);

        while (true)
        {
            yield return new WaitForSeconds(_currentSource.clip.length - _crossfadeLength);

            _previousSource = _currentSource;
            _currentSource = (_currentSource == _gameplaySource_01) ? _gameplaySource_02 : _gameplaySource_01;

            var prevSrc = _previousSource;
            DOTween.Kill(prevSrc);
            prevSrc.DOFade(0, _crossfadeLength * 2).OnComplete(() => prevSrc.Stop());

            DOTween.Kill(_currentSource);
            _currentSource.clip = GetRandomGameplaySong();
            _currentSource.volume = 0;
            _currentSource.Play();
            _currentSource.DOFade(TargetVolume, _crossfadeLength * 2);

            yield return new WaitForSeconds(_crossfadeLength * 2);
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
    public AudioClip GameOverSong;
    public AudioClip DayChangeSong;

    public List<AudioClip> GameplaySongs = new();
}
