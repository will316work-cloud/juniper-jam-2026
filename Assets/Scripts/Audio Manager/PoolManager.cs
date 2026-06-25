using System;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public SfxVolumeSettings[] SfxVolumeSettings = new SfxVolumeSettings[Enum.GetValues(typeof(AudioType)).Length];

    [Space(15)]
    [Range(0, 1)] public float OverallVolume_Song = 0.7f;
    [Range(0, 1)] public float OverallVolume_SFX = 0.7f;
    [Range(0, 1)] public float MasterVolume = 0.7f;

    public SfxPooler SfxPooler = new();
    public SongPooler MusicPooler = new();

    public SfxPoolData AudioPoolData = new();
    public SongPoolerData SongPoolerData = new();
    public PlayerMovementSoundHandler PlayerMovementSoundHandler = new();
    [Space(10)]
    [Header("Player Chair Sound")]
    public AudioClip ChairSound;
    [Range(0, 1)] public float ChairVolume = 0.7f;

    public void Initialize(GameContext ctx)
    {
        SfxPooler.Initialize(AudioPoolData, this, SfxVolumeSettings);
        MusicPooler.Initialize(SongPoolerData, this);
        PlayerMovementSoundHandler.Initialize(ctx.PlayerControl, SfxPooler.CreateNewSource(),ChairSound);
        
        OnOverallVolumeChange();
    }

    void Update()
    {
        PlayerMovementSoundHandler.Tick();
    }

    public void GetSfx(AudioType audioType, bool randomPitch = true) => SfxPooler.GetAudio(audioType, randomPitch);
    public void FadeInMenuMusic() => MusicPooler.FadeInMenuMusic();
    public void FadeOutMenuMusic() => MusicPooler.FadeOutMenuMusic();
    public void FadeInGameplayMusic() => MusicPooler.FadeInGameplayMusic();

    public void FadeInGameOverMusic() => MusicPooler.FadeInGameOverMusic();
    public void FadeOutGameOverMusic() => MusicPooler.FadeOutGameOverMusic();

    public void FadeInDayChangeMusic() => MusicPooler.FadeInDayChangeMusic();
    public void FadeOutDayChangeMusic() => MusicPooler.FadeOutDayChangeMusic();

    public void OnOverallVolumeChange()
    {
        SfxPooler.SetOverallVolume(MasterVolume * OverallVolume_SFX);
        MusicPooler.SetOverallVolume(MasterVolume * OverallVolume_Song);
        PlayerMovementSoundHandler.Setvolume(MasterVolume * OverallVolume_SFX * ChairVolume);
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
        PlayerMovementSoundHandler.Setvolume(MasterVolume * OverallVolume_SFX * ChairVolume);
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