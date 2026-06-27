using System.Collections;
using UnityEngine;

public class StunHandler : MonoBehaviour
{
    public float StunDuration;
    public float StunProtectionDuration;

    private bool _isStunned; public bool IsStunned => _isStunned;
    private bool _isProtectedFromStun; public bool IsProtectedFromStun => _isProtectedFromStun;

    PlayerControl _playerControl;

    public bool IsDebugOn;
    GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _playerControl = ctx.PlayerControl;
        _ctx = ctx;
    }

    IEnumerator StunRoutine()
    {
        _ctx.PoolManager.GetSfx(AudioType.PlayerIsHit);
        _ctx.CameraController.ShakeCamera(EffectType.Stunned);
        if(IsDebugOn) Debug.Log("Player is stunned");
        _playerControl.AddMovementBlockReason(MovementBlockReason.Stun);
        _isStunned = true;

        yield return new WaitForSeconds(StunDuration);
        if(IsDebugOn) Debug.Log("Player is not stunned anymore. Player is stun protected");
        _isStunned = false;
        _isProtectedFromStun = true;
        _playerControl.RemoveMovementBlockReason(MovementBlockReason.Stun);
        StartCoroutine(StunProtectionRoutine());
    }

    IEnumerator StunProtectionRoutine()
    {
        yield return new WaitForSeconds(StunProtectionDuration);
        _isProtectedFromStun = false;
        if(IsDebugOn) Debug.Log("Stun protection is over");
    }

    public void ActivateStunProtection() => StartCoroutine(StunProtectionRoutine());

    void OnTriggerEnter(Collider other)
    {
        if(!_isProtectedFromStun && !_isStunned && other.TryGetComponent(out Coworker coworker) && coworker.IsMoving)
            StartCoroutine(StunRoutine());
    }
}