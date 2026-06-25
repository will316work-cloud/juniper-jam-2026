using DG.Tweening;
using UnityEngine;

public class PlayerMovementSoundHandler
{
    AudioSource _source;
    PlayerControl _player;
    bool _isMoving;
    float _volume = 0.7f;

    public void Setvolume(float volume)
    {
        _volume = volume;
        _source.volume = _volume;
    }

    public void Initialize(PlayerControl player, AudioSource source, AudioClip chairSound)
    {
        _player = player;
        _source = source;
        _source.name = "Chair Sound";
        _source.playOnAwake = false;
        _source.clip = chairSound;
        _source.loop = true;
    }

    public void Tick()
    {
        if(!_isMoving && Mathf.Abs(_player.Rb.linearVelocity.x) > 0.1f || Mathf.Abs(_player.Rb.linearVelocity.z) > 0.1f)
        {
            StartChairSound();
        }
        else if(_isMoving && Mathf.Abs(_player.Rb.linearVelocity.x) < 0.1f && Mathf.Abs(_player.Rb.linearVelocity.z) < 0.1f)
        {
            StopChairSound();
        }
    }

    public void StartChairSound()
    {
        _isMoving = true;
        if(_source.isPlaying) return;
        _source.Play();
        _source.DOKill();
        _source.DOFade(_volume, 0.05f);
    }

    public void StopChairSound()
    {
        _source.DOKill();
        _source.DOFade(0, 0.1f).OnComplete(() => _source.Stop());
        _isMoving = false;
    }
}