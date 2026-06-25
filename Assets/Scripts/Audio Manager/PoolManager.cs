using System;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public SfxVolumeSettings[] SfxVolumeSettings = new SfxVolumeSettings[Enum.GetValues(typeof(AudioType)).Length];

    [Range(0, 1)] public float OverallVolume_Song = 0.7f;
    [Range(0, 1)] public float OverallVolume_SFX = 0.7f;
    [Range(0, 1)] public float MasterVolume = 0.7f;

    public SfxPooler SfxPooler = new();
    public SongPooler MusicPooler = new();

    public SfxPoolData AudioPoolData = new();
    public SongPoolerData SongPoolerData = new();
    

    public void Initialize()
    {
        SfxPooler.Initialize(AudioPoolData, this, SfxVolumeSettings);
        MusicPooler.Initialize(SongPoolerData, this);
        
        OnOverallVolumeChange();
    }

    public void GetSfx(AudioType audioType, bool randomPitch = true) => SfxPooler.GetAudio(audioType, randomPitch);
    public void FadeInMenuMusic() => MusicPooler.FadeInMenuMusic();
    public void FadeOutMenuMusic() => MusicPooler.FadeOutMenuMusic();
    public void FadeInGameplayMusic() => MusicPooler.FadeInGameplayMusic();

    public void OnOverallVolumeChange()
    {
        SfxPooler.SetOverallVolume(MasterVolume * OverallVolume_SFX);
        MusicPooler.SetOverallVolume(MasterVolume * OverallVolume_Song);
    }

    private void OnValidate()
    {
        MasterVolume = Mathf.Clamp01(MasterVolume);
        // optionally apply immediately in edit mode:
        // if(Application.isPlaying) OnOverallVolumeChange();
    }

    public void OnMasterVolumeChange(float volume)
    {
        MasterVolume = volume;
        OnOverallVolumeChange();
    }

    public void OnSfxVolumeChange(float volume)
    {
        OverallVolume_SFX = volume;
        SfxPooler.SetOverallVolume(MasterVolume * OverallVolume_SFX);
    }

    public void OnSongVolumeChange(float volume)
    {
        OverallVolume_Song = volume;
        MusicPooler.SetOverallVolume(MasterVolume * OverallVolume_Song);
    }



}

[Serializable]
public class SfxVolumeSettings
{
    public AudioType Type;
    [Range(0, 1)] public float Volume;
}