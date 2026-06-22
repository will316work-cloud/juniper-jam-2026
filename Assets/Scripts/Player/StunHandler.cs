using System.Collections;
using UnityEngine;

public class StunHandler : MonoBehaviour
{
    public float StunDuration;
    public float StunProtectionDuration;

    private bool _isStunned; public bool IsStunned => _isStunned;
    private bool _isProtectedFromStun; public bool IsProtectedFromStun => _isProtectedFromStun;

    PlayerControl _playerControl;

    public void Initialize(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    IEnumerator StunRoutine()
    {
        Debug.Log("Player is stunned");
        _playerControl.AddMovementBlockReason(MovementBlockReason.Stun);
        _isStunned = true;

        yield return new WaitForSeconds(StunDuration);
        Debug.Log("Player is not stunned anymore. Player is stun protected");
        _isStunned = false;
        _isProtectedFromStun = true;
        _playerControl.RemoveMovementBlockReason(MovementBlockReason.Stun);
        StartCoroutine(StunProtectionRoutine());
    }

    IEnumerator StunProtectionRoutine()
    {
        yield return new WaitForSeconds(StunProtectionDuration);
        _isProtectedFromStun = false;
        Debug.Log("Stun protection is over");
    }

    void OnTriggerEnter(Collider other)
    {
        if(!_isProtectedFromStun && !_isStunned && other.TryGetComponent(out Coworker coworker) && coworker.IsMoving)
            StartCoroutine(StunRoutine());
    }
}